using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.Gyroscope;


public class PaddleMovement : MonoBehaviour
{
    private Collider2D[] colliders = new Collider2D[3];
    
    [SerializeField] private float speed;
    [SerializeField] private PaddleData paddleData;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] public CollisionCheck collisionCheck;
    [SerializeField] private LayerMask limitLayer;
    
    private bool movementEnabled;
    void Start()
    {
        if (SystemInfo.supportsLocationService)
        {
            Input.compass.enabled = true;
            movementEnabled = true;
        }
        else
        {
            Debug.LogError("Gyro not supported");
            Input.compass.enabled = false;
        }
    }
    void Movement()
    {
        if (Input.compass.enabled && movementEnabled)
        {
            Vector3 magneticField = Input.compass.rawVector;
            
            float magnetX = magneticField.x;
            float normalizedY = Mathf.Clamp(magnetX / 50f, -1f, 1f);
            transform.Translate(0f, -1f * normalizedY * speed * Time.deltaTime, 0f);
        }
    }

    private void FixedUpdate()
    {
        Physics2D.OverlapCircleNonAlloc(transform.position, paddleData.CollisionRadius, colliders, limitLayer);
        Movement();
    }

    private void LateUpdate()
    {
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        foreach (BoxCollider2D collider in colliders)
        {
            if (collider != null)
            {
                if (collider.gameObject != this.gameObject && collisionCheck.RectangleCollision(playerCollider, collider))
                {
                    Debug.Log("Collision Borde");
                    movementEnabled = false;
                }
                else
                {
                    movementEnabled = true;
                }
                
                if(collider.gameObject != this.gameObject)Debug.Log("Collider " + collider.gameObject.name + " entro en radio de deteccion");
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, paddleData.CollisionRadius);
    }
}
