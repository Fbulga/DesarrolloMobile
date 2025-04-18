using UnityEngine;


[CreateAssetMenu(fileName = "PaddleData", menuName = "Data/PaddleData", order = 0)]
public class PaddleData : ScriptableObject
{
    [SerializeField] private float speed;
    public float Speed => speed;
    
    [SerializeField] private float collisionRadius;
    public float CollisionRadius => collisionRadius;
    
    [SerializeField, Range(0f,2f)] 
    private float movementSensitivity;
    public float MovementSensitivity => movementSensitivity;
    
    [SerializeField] private float rayDistance;
    public float RayDistance => rayDistance;
    
    [SerializeField] private LayerMask limitLayerMask;
    public LayerMask LimitLayerMask => limitLayerMask;
}
