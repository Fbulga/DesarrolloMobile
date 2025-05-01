using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Brick : MonoBehaviour, IBreakable
{
    [SerializeField] private int health;
    [SerializeField] private BrickData data;
    [SerializeField] private int brickPoints;
    private float dropChance;
    
    private bool destroyed = false;

    private void Start()
    {
        ArkanoidManager.Instance.OnNewBrick?.Invoke();
    }

    public void DestroyMe()
    {
        if (destroyed) return;
        TryDropPowerUp();
        destroyed = true;
        ArkanoidManager.Instance.OnDestroyBrick?.Invoke(brickPoints);
        Destroy(gameObject);            
    }

    public void TryDestroyMe()
    {
        health--;
        if (health <= 0) DestroyMe();
    }

    public void TryDropPowerUp()
    {
        dropChance = Random.value;
        if (dropChance <= data.DropChance && data.PowerUps.Length > 0f)
        {
            EffectSO effectSO = data.PowerUps[Random.Range(0, data.PowerUps.Length)];
            GameObject obj = PowerUpPoolManager.Instance.GetPowerUp(effectSO.Prefab, transform.position);
            PowerUp powerUp = obj.GetComponent<PowerUp>();
            powerUp.powerUpEffect = effectSO;
        }
    }
}
