using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.Gyroscope;


public class PaddleMovement : MonoBehaviour
{
    [SerializeField] public PaddleData paddleData;
    public float rayDistance;
    
    private bool leftMovementBlocked = false;
    private float leftMovement;
    public float LeftMovement => leftMovement;
    private bool rightMovementBlocked = false;
    private float rightMovement;
    public float RightMovement => rightMovement;
    
    private float normalizedX;


    void Start()
    {
        leftMovement = -1f;
        rightMovement = 1f;
        rayDistance = paddleData.RayDistance;
        

    }
    private void FixedUpdate()
    {
        
        CollisionRays(out leftMovementBlocked,out rightMovementBlocked);
    }

    private void LateUpdate()
    {
        Movement();
    }


    void Movement()
    {
        if (GameManager.Instance.IsMobilePlatform)
        {
            /*Vector3 magneticField = Input.compass.rawVector;
            float magnetX = magneticField.x;
            normalizedX = Mathf.Clamp(magnetX / 50f, leftMovement,rightMovement);*/
            float tiltX = Input.acceleration.x;
            //normalizedX = Mathf.Lerp(normalizedX, Mathf.Clamp(tiltX, leftMovement, rightMovement), 0.1f);
            normalizedX = Mathf.Clamp(tiltX, leftMovement, rightMovement);
        }
        else
        {
            float inputX = Input.GetAxis("Horizontal");
            normalizedX = Mathf.Clamp(inputX, leftMovement, rightMovement);
        }

        //transform.Translate(0f, -1f * normalizedX * paddleData.Speed * Time.deltaTime * paddleData.MovementSensitivity, 0f);
        transform.Translate(normalizedX * paddleData.Speed * Time.deltaTime * paddleData.MovementSensitivity, 0f, 0f); //por si vuelves a girar el paddle a 90!!

        

    }

    
    public bool CollisionRays(out bool leftMovementBlocked, out bool rightMovementBlocked)
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, paddleData.LimitLayerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, rayDistance, paddleData.LimitLayerMask);
        
        
        if (hitRight.collider != null)
        {
            Debug.Log("toco el limite a la derecha: " + hitRight.collider.name);
            rightMovement = 0f;
            rightMovementBlocked = true;
        }
        else
        {
            rightMovement = 1f;
            rightMovementBlocked = false;
        }
        
        if (hitLeft.collider != null)
        {
            Debug.Log("toco el limite a la izquierda: " + hitLeft.collider.name);
            leftMovement = 0f;
            leftMovementBlocked = true;
        }
        else
        {
            leftMovement = -1f;
            leftMovementBlocked = false;
        }
        
        return leftMovementBlocked || rightMovementBlocked;
    }
    
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y) + Vector2.right * rayDistance);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y) + Vector2.left * rayDistance);
    }
}
