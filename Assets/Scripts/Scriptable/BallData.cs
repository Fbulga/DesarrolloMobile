using UnityEngine;


[CreateAssetMenu(fileName = "BallData", menuName = "Data/BallData", order = 1)]
public class BallData : ScriptableObject
{
    [SerializeField] private float speed;
    public float Speed => speed;
    
    [SerializeField] private float collisionRadius;
    public float CollisionRadius => collisionRadius;
    
    [SerializeField] private LayerMask obstacleLayer;

    public LayerMask ObstacleLayer => obstacleLayer;
}
