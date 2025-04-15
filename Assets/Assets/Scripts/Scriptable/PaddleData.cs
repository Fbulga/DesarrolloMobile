using UnityEngine;


[CreateAssetMenu(fileName = "PaddleData", menuName = "Paddle", order = 0)]
public class PaddleData : ScriptableObject
{
    [SerializeField] private float speed;
    public float Speed => speed;
    
    [SerializeField] private float collisionRadius;
    public float CollisionRadius => collisionRadius;
    
    
}
