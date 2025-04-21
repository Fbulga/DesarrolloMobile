using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float speed;
    private float detectionRadius;
    
    [SerializeField] private EffectSO powerUpEffect;
    [SerializeField] private Collider2D[] colliders = new Collider2D[2];
    [SerializeField] private CollisionCheck collisionCheck;
    private CircleCollider2D circleCollider2D;
    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        speed = powerUpEffect.Speed;
        detectionRadius = powerUpEffect.DetectionRadius;
    }

    void Update()
    {
        transform.position += (Vector3)(new Vector2(0f,-1f) * (speed * Time.deltaTime));
        Physics2D.OverlapCircleNonAlloc(transform.position,detectionRadius,colliders);
        CheckCollisions();
    }
    
    private void CheckCollisions()
    {
        foreach (var collider in colliders)
        {
            if (collider == null) return;
            
            var response = collisionCheck.SphereRectangleCollision(collider,circleCollider2D);
            if (response)
            {
                collider.TryGetComponent<DeadZone>(out DeadZone deadZone);
                if (deadZone != null)
                {
                    if (deadZone.IsDeadly)
                    {
                        DeactivatePowerUp();
                        Debug.Log("Collision Detected");
                    }
                }
                
                collider.TryGetComponent<PaddleEffect>(out PaddleEffect paddle);
                if (paddle != null)
                {
                    powerUpEffect.Execute(paddle.gameObject);
                    DeactivatePowerUp();
                }
            }
        }
    }

    void DeactivatePowerUp()
    {
        Destroy(gameObject);
    }
    
    


}
