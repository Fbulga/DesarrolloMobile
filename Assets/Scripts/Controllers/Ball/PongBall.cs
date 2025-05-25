using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PongBall : BaseBall
{
    private int deviateCounter = 0;
    private float maxSpeedFactor;
    private float speedIncreaseRatio;

    protected override void Start()
    {
        base.Start();
        speedIncreaseRatio = PongManager.Instance.SpeedIncreaseRatio;
        maxSpeedFactor = PongManager.Instance.MaxSpeedFactor;
    }
    
    protected override void HandleCollision(RaycastHit2D hit)
    {
        Collider2D collider = hit.collider;

        if (collider.TryGetComponent<ScoreZone>(out ScoreZone scoreZone))
        {
            if (scoreZone.IsPlayer)
            {
                PongManager.Instance.OnIAScored?.Invoke();
            }
            else
            {
                PongManager.Instance.OnPlayerScored?.Invoke();
            }

            ResetBall();
            return;
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("IA"))
        {
            if (currentSpeed < ballData.Speed * maxSpeedFactor)
            {
                currentSpeed *= 1f + (speedIncreaseRatio / 100f);
            }
        }
        
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (currentSpeed < ballData.Speed * maxSpeedFactor)
            {
                currentSpeed *= 1f + (speedIncreaseRatio / 100f);
            }
            Vector2 newDir = CalculatePaddleBounce(hit.point, collider.transform);
            SetDirection(newDir);
            deviateCounter++;
            if (deviateCounter >= 3)
            { 
                DeviateBall();
            }
            ballVisuals.HandleBounceEffect();
            return;
        }
        
        base.HandleCollision(hit);
    }
    
    private void ResetBall()
    {
        ballVisuals.HandleBounceEffect();
        ballVisuals.SpriteRenderer.enabled = false;
        
        if (GameManager.Instance.IsMobilePlatform)
        {
            if (ballVisuals.TrailRenderer != null && ballVisuals.TrailRenderer.enabled)
            {
                ballVisuals.TrailRenderer.enabled = false;
            }
        }
        
        deviateCounter = 0;
        currentSpeed = ballData.Speed;
        transform.position = PongManager.Instance.BallSpawn.transform.position;
        ballVisuals.SpriteRenderer.enabled = true;

        if (GameManager.Instance.IsMobilePlatform)
        {
            if (ballVisuals.TrailRenderer != null && !ballVisuals.TrailRenderer.enabled)
            {
                ballVisuals.TrailRenderer.enabled = true;
            }
        }
        
        
        SetDirection(new Vector2(Random.Range(-1f, 1f), direction.y*-1f));
    }
    
    private void DeviateBall()
    {
        deviateCounter = 0;
        SetDirection(new Vector2(direction.normalized.x * Random.Range(-1f,1f), direction.normalized.y));
    }

    
}

