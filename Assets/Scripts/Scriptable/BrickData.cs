using UnityEngine;

[CreateAssetMenu(fileName = "BrickData", menuName = "Data/BrickData", order = 0)]
public class BrickData : ScriptableObject
{
    [SerializeField] private AudioClip clip;
    public AudioClip Clip => clip;
    
    [SerializeField] private GameObject[] powerUps;
    public GameObject[] PowerUps => powerUps;
}
