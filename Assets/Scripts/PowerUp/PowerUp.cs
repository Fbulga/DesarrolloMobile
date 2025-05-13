using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float speed;
    private float detectionRadius;
    
    [SerializeField] public EffectSO powerUpEffect;
    [SerializeField] private Collider2D[] colliders = new Collider2D[2];
    [SerializeField] private CollisionCheck collisionCheck;
    private CircleCollider2D circleCollider2D;
    private float lifeSpan = 5f;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = powerUpEffect.PowerUpColor;
        circleCollider2D = GetComponent<CircleCollider2D>();
        speed = powerUpEffect.Speed;
        detectionRadius = powerUpEffect.DetectionRadius;
        StartCoroutine(DeactivatePowerUpAfterLifeSpan());
    }

    void Update()
    {
        Physics2D.OverlapCircleNonAlloc(transform.position,detectionRadius,colliders);
        CheckCollisions();
        transform.position += (Vector3)(speed * new Vector2(0f,-1f) * Time.deltaTime);
    }
    
    private void CheckCollisions()
    {
        foreach (var collider in colliders)
        {
            if (collider == null) return;
            
            var response = collisionCheck.SphereRectangleCollision(collider,circleCollider2D);
            if (response)
            {
                collider.TryGetComponent<DeadZone>(out DeadZone deadZone);
                if (deadZone != null)
                {
                        DeactivatePowerUp();
                }
                
                collider.TryGetComponent<PaddleEffect>(out PaddleEffect paddle);
                if (paddle != null)
                {
                    powerUpEffect.Execute(paddle.gameObject);
                    StatManager.Instance.IncreaseStat(Stat.PowerUpsCollected,1f);
                    DeactivatePowerUp();
                }
            }
        }
    }

    void DeactivatePowerUp()
    {
        Particles(powerUpEffect.PowerUpColor);
        SFXManager.Instance.PlaySFXClip(SFXType.PickPowerUp);
        PoolManager.Instance.ReturnPowerUp(this.gameObject,powerUpEffect.Prefab);
    }

    IEnumerator DeactivatePowerUpAfterLifeSpan()
    {
        yield return new WaitForSeconds(lifeSpan);
        DeactivatePowerUp();
    }
    
    private void Particles(Color color)
    {
        //TODO: Le cambio del color al prefab del SO, crear un particle segun color y enum
        var ps = powerUpEffect.ParticlesPrefab.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = color;
        PoolManager.Instance.GetPowerUp(powerUpEffect.ParticlesPrefab, transform.position);
    }
    

}
