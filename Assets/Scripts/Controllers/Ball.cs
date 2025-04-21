using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Ball : MonoBehaviour
{   
    [SerializeField] private CollisionCheck collisionCheck;
    [SerializeField] private BallData ballData;
    
    private Collider2D[] colliders = new Collider2D[5];
    private CircleCollider2D circleCollider2D;
    
    private Vector2 direction;
    public Vector2 Direction => direction;


    private void Start()
    {
        GameManager.Instance.NewBallInGame(this.gameObject);
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = ballData.CollisionRadius;
        direction = new Vector2(Random.Range(-1f,1f), -1f);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * (ballData.Speed * Time.deltaTime));
        Physics2D.OverlapCircleNonAlloc(transform.position,ballData.CollisionRadius,colliders,ballData.ObstacleLayer);
        CheckCollisions();
    }

    private void CheckCollisions()
    {
        foreach (var collider in colliders)
        {
            if (collider == null) return;
            
            var response = collisionCheck.SphereRectangleCollisionStruct(collider,circleCollider2D);
            if (response.isTouching)
            {
                collider.TryGetComponent<DeadZone>(out DeadZone deadZone);
                if (deadZone != null)
                {
                    if (deadZone.IsDeadly)
                    {
                        DeactivateBall();
                        Debug.Log("Collision Detected");
                    }
                }
                
                
                collider.TryGetComponent<IBreakable>(out IBreakable brick);
                if (brick != null) brick.TryDestroyMe();
                
                Vector2 normal = response.collisionNormal;
                direction = Vector2.Reflect(direction, normal).normalized;
                transform.position = response.closestPoint + normal * ((circleCollider2D.radius) + 0.025f);

                if (Mathf.Abs(direction.x) < 0.01f)
                {
                    direction.x = Random.Range(-0.5f, 0.5f);
                    direction = direction.normalized;
                }
                else if (Mathf.Abs(direction.y) < 0.01f)
                {
                    direction.y = Random.Range(-0.5f, 0.5f);
                    direction = direction.normalized;
                }
                
            }
        }
    }

    private void DeactivateBall()
    {
        GameManager.Instance.RemoveBall(this.gameObject);
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,ballData.CollisionRadius);
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }
}
