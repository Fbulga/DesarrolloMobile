using UnityEngine;

public abstract class EffectSO : ScriptableObject, IEffectable
{
    
    protected virtual float speed => 2f;
    public float Speed => speed;
    
    protected float detectionRadius => 0.4f;
    public float DetectionRadius => detectionRadius;
    
    
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;
    
    public abstract void Execute(GameObject target);
}