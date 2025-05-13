using Enum;
using UnityEngine;
using Random = UnityEngine.Random;

public class Brick : MonoBehaviour, IBreakable
{
    [SerializeField] private int health;
    [SerializeField] private BrickData data;
    [SerializeField] private int brickPoints;
    private float dropChance;
    private SpriteRenderer spriteRenderer;
    
    private bool destroyed = false;
    
    private void Start()
    {
        ArkanoidManager.Instance.OnNewBrick?.Invoke();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = data.LifeColors[health];
    }

    public void DestroyMe()
    {
        if (destroyed) return;
        destroyed = true;
        VibrationManager.VibrateMedium();
        ArkanoidManager.Instance.OnDestroyBrick?.Invoke(brickPoints);
        StatManager.Instance.IncreaseStat(Stat.BricksDestroyed,1f);
        Destroy(gameObject);            
    }

    public void TryDestroyMe()
    {
        health--;
        Particles(spriteRenderer.color);
        TryDropPowerUp();
        UpdateColor(health);
        SFXManager.Instance.PlaySFXClip(data.BrickSFX);
        if (health <= 0) DestroyMe();
    }

    public void TryDropPowerUp()
    {
        dropChance = Random.value;
        if (dropChance <= data.DropChance && data.PowerUps.Length > 0f)
        {
            EffectSO effectSO = data.PowerUps[Random.Range(0, data.PowerUps.Length)];
            GameObject obj = PoolManager.Instance.GetPowerUp(effectSO.PrefabType, transform.position);
            PowerUp powerUp = obj.GetComponent<PowerUp>();
            powerUp.powerUpEffect = effectSO;
        }
    }

    private void UpdateColor(int health)
    {
        spriteRenderer.color = data.LifeColors[health];
    }

    private void Particles(Color color)
    {
        GameObject particleGO = PoolManager.Instance.GetPowerUp(PrefabsType.BrickParticles, transform.position);
        ParticleSystem ps = particleGO.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule psMain = ps.main;
        psMain.startColor = color;
        Particles particleType = particleGO.GetComponent<Particles>();
        particleType.prefabType = PrefabsType.BrickParticles;

    }
}
