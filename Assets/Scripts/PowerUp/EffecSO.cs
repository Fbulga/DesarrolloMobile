using UnityEngine;

public abstract class EffectSO : ScriptableObject, IEffectable
{
    
    protected virtual float speed => 2f;
    public float Speed => speed;
    
    [SerializeField] private float detectionRadius;
    public float DetectionRadius => detectionRadius;
    
    
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;
    
    
    [SerializeField] private GameObject particlesPrefab;
    public GameObject ParticlesPrefab => particlesPrefab;
    
    [SerializeField] private Color powerUpColor;
    public Color PowerUpColor => powerUpColor;
    public abstract void Execute(GameObject target);
}