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
    
    private bool destroyed = false;

    private void Start()
    {
        GameManager.Instance.NewBrickInGame();
    }

    public void DestroyMe()
    {
        if (destroyed) return;
        TryDropPowerUp();
        destroyed = true;
        GameManager.Instance.DestroyBrick(brickPoints);
        Destroy(gameObject);
    }

    public void TryDestroyMe()
    {
        health--;
        if (health <= 0) DestroyMe();
    }

    public void TryDropPowerUp()
    {
        if (data.PowerUps.Length > 0f)
        {
            Instantiate(data.PowerUps[Random.Range(0, data.PowerUps.Length)], transform.position, Quaternion.identity);
            Debug.Log("PowerUp Detected");
        }
    }
}
