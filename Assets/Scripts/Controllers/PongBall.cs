using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PongBall : MonoBehaviour
{
    [SerializeField] private CollisionCheck collisionCheck;
    [SerializeField] private BallData ballData;
    
    private Collider2D[] colliders = new Collider2D[5];
    private CircleCollider2D circleCollider2D;
    
    private Vector2 direction;
    public Vector2 Direction => direction;

    [SerializeField] private float speedIncreaseRatio;
    private float speed;
    
    
    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = ballData.CollisionRadius;
        direction = new Vector2(Random.Range(-1f,1f), -1f);
        speed = ballData.Speed;
        speedIncreaseRatio = PongManager.Instance.speedIncreaseRatio;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * (speed * Time.deltaTime));
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
                collider.TryGetComponent<ScoreZone>(out ScoreZone scoreZone);
                if (scoreZone != null)
                {
                    if (scoreZone.IsPlayer)
                    {
                        PongManager.Instance.OnIAScored?.Invoke();
                        ScoreBall();
                        Debug.Log("Collision Detected");
                    }
                    else
                    {
                        PongManager.Instance.OnPlayerScored?.Invoke();
                        ScoreBall();
                        Debug.Log("Collision Detected");
                    }
                    return;
                }


                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (speed < ballData.Speed * 2.25f)
                    {
                        speed *= (1f + speedIncreaseRatio/100);
                    }
                }
                
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

    private void ScoreBall()
    {
        speed = ballData.Speed;
        transform.position =  PongManager.Instance.BallSpawn.transform.position;
        SetDirection(direction * new Vector2(0f,-1f));
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
