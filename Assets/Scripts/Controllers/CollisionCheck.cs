using UnityEngine;
using System;

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
}
