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
    

    private float leftMovement;
    private float rightMovement;
    void Start()
    {
        Input.compass.enabled = true;
        leftMovement = -1f;
        rightMovement = 1f;
        rayDistance = paddleData.RayDistance;
    }
    private void Update()
    {
        Movement();
    }
    void Movement()
    {
        Vector3 magneticField = Input.compass.rawVector;
        float magnetX = magneticField.x;
        float normalizedX = Mathf.Clamp(magnetX / 50f, leftMovement,rightMovement);
        transform.Translate(0f, -1f * normalizedX * paddleData.Speed * Time.deltaTime * paddleData.MovementSensitivity, 0f);
    }


    private void LateUpdate()
    {
        CollisionRays();
    }
    
    private void CollisionRays()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, paddleData.LimitLayerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, rayDistance, paddleData.LimitLayerMask);
        
        
        if (hitRight.collider != null)
        {
            Debug.Log("toco el limite a la derecha: " + hitRight.collider.name);
            rightMovement = 0f;
        }
        else
        {
            rightMovement = 1f;
        }
        
        if (hitLeft.collider != null)
        {
            Debug.Log("toco el limite a la izquierda: " + hitLeft.collider.name);
            leftMovement = 0f;
        }
        else
        {
            leftMovement = -1f;
        }
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y) + Vector2.right * rayDistance);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y) + Vector2.left * rayDistance);
    }
}
