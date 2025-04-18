using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    
    public bool SphereRectangleCollision(Collider2D rec, CircleCollider2D circle)
    {
        Vector2 closestPoint = circle.transform.position;
        if (closestPoint.x < rec.bounds.min.x) closestPoint.x = rec.bounds.min.x;
        if (closestPoint.x > rec.bounds.max.x) closestPoint.x = rec.bounds.max.x;
        if (closestPoint.y < rec.bounds.min.y) closestPoint.y = rec.bounds.min.y;
        if (closestPoint.y > rec.bounds.max.y) closestPoint.y = rec.bounds.max.y;

        return Vector2.Distance(closestPoint, circle.transform.position) < circle.radius;
    }
    
    public CollisionResponseDto SphereRectangleCollisionStruct(Collider2D rec, CircleCollider2D circle)
    {
        Vector2 closestPoint = circle.transform.position;

        if (closestPoint.x < rec.bounds.min.x) closestPoint.x = rec.bounds.min.x;
        if (closestPoint.x > rec.bounds.max.x) closestPoint.x = rec.bounds.max.x;
        if (closestPoint.y < rec.bounds.min.y) closestPoint.y = rec.bounds.min.y;
        if (closestPoint.y > rec.bounds.max.y) closestPoint.y = rec.bounds.max.y;

        bool isTouching = Vector2.Distance(closestPoint, circle.transform.position) < circle.radius;

        Vector2 collisionNormal = Vector2.zero;

        if (isTouching)
        {
            float deltaX = Mathf.Abs(closestPoint.x - circle.transform.position.x);
            float deltaY = Mathf.Abs(closestPoint.y - circle.transform.position.y);

            if (Mathf.Approximately(deltaX, deltaY))
            {
                collisionNormal = (circle.transform.position - (Vector3)closestPoint).normalized;

            }
            else if (deltaY > deltaX)
            {
                collisionNormal = new Vector2(0, Mathf.Sign(circle.transform.position.y - closestPoint.y));
            }
            else
            {
                collisionNormal = new Vector2(Mathf.Sign(circle.transform.position.x - closestPoint.x), 0);
            }
        }

        return new CollisionResponseDto()
        {
            closestPoint = closestPoint,
            isTouching = isTouching,
            collisionNormal = collisionNormal
        };
    }
}
