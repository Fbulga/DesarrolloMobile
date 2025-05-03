using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseBall : MonoBehaviour
{
    [SerializeField] protected CollisionCheck collisionCheck;
    [SerializeField] protected BallData ballData;
    protected Collider2D[] colliders = new Collider2D[5];
    protected CircleCollider2D circleCollider2D;

    protected Vector2 direction;
    public Vector2 Direction => direction;

    protected virtual void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.radius = ballData.CollisionRadius;
        direction = new Vector2(Random.Range(-1f, 1f), -1f);
    }

    protected virtual void Update()
    {
        MoveBall();
        Physics2D.OverlapCircleNonAlloc(transform.position, ballData.CollisionRadius, colliders, ballData.ObstacleLayer);
        CheckCollisions();
    }

    protected void MoveBall()
    {
        transform.position += (Vector3)(GetSpeed() * direction * Time.deltaTime);
    }
    
    protected void Reflect(CollisionResponseDto response)
    {
        Vector2 normal = response.collisionNormal;
        direction = Vector2.Reflect(direction, normal).normalized;
        transform.position = response.closestPoint + normal * (circleCollider2D.radius + 0.025f);

        if (Mathf.Abs(direction.x) < 0.01f)
            direction.x = Random.Range(-0.5f, 0.5f);

        if (Mathf.Abs(direction.y) < 0.01f)
            direction.y = Random.Range(-0.5f, 0.5f);

        direction = direction.normalized;
    }
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    protected abstract float GetSpeed();
    protected abstract void CheckCollisions();
    
}

