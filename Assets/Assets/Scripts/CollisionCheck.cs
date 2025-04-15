using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public bool RectangleCollision(Collider2D rec1, BoxCollider2D rec2)
    {
        if (rec1.bounds.min.x < rec2.bounds.max.x &&
            rec2.bounds.min.x < rec1.bounds.max.x &&
            rec1.bounds.min.y < rec2.bounds.max.y &&
            rec2.bounds.min.y < rec1.bounds.max.y)
            return true;

        return false;
    }
    
    public bool SphereRectangleCollision(Collider2D rec, CircleCollider2D sphere)
    {
        Vector2 closestPoint = sphere.transform.position;
        if (closestPoint.x < rec.bounds.min.x) closestPoint.x = rec.bounds.min.x;
        if (closestPoint.x > rec.bounds.max.x) closestPoint.x = rec.bounds.max.x;
        if (closestPoint.y < rec.bounds.min.y) closestPoint.y = rec.bounds.min.y;
        if (closestPoint.y > rec.bounds.max.y) closestPoint.y = rec.bounds.max.y;

        return Vector2.Distance(closestPoint, sphere.transform.position) < sphere.radius;
    }
    
    public CollisionResponseDto SphereRectangleCollisionStruct(Collider2D rec, CircleCollider2D sphere)
    {
        Vector2 closestPoint = sphere.transform.position;

        if (closestPoint.x < rec.bounds.min.x) closestPoint.x = rec.bounds.min.x;
        if (closestPoint.x > rec.bounds.max.x) closestPoint.x = rec.bounds.max.x;
        if (closestPoint.y < rec.bounds.min.y) closestPoint.y = rec.bounds.min.y;
        if (closestPoint.y > rec.bounds.max.y) closestPoint.y = rec.bounds.max.y;

        bool isTouching = Vector2.Distance(closestPoint, sphere.transform.position) < sphere.radius;

        Vector2 collisionNormal = Vector2.zero;

        if (isTouching)
        {
            float deltaX = Mathf.Abs(closestPoint.x - sphere.transform.position.x);
            float deltaY = Mathf.Abs(closestPoint.y - sphere.transform.position.y);

            if (Mathf.Approximately(deltaX, deltaY))
            {
                collisionNormal = (sphere.transform.position - (Vector3)closestPoint).normalized;

            }
            else if (deltaY > deltaX)
            {
                collisionNormal = new Vector2(0, Mathf.Sign(sphere.transform.position.y - closestPoint.y));
            }
            else
            {
                collisionNormal = new Vector2(Mathf.Sign(sphere.transform.position.x - closestPoint.x), 0);
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
