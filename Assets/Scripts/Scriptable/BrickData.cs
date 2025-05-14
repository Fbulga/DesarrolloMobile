using Enum;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BrickData", menuName = "Data/BrickData", order = 0)]
public class BrickData : ScriptableObject
{
    [SerializeField] private EffectSO[] powerUps;
    public EffectSO[] PowerUps => powerUps;

    [SerializeField]
    [Range(0f,1f)]private float dropChance;
    public float DropChance => dropChance;
    [SerializeField] private Color[] lifeColors;
    public Color[] LifeColors => lifeColors;
        
    [SerializeField] private SFXType brickSFX;
    public SFXType BrickSFX => brickSFX;
    
}
