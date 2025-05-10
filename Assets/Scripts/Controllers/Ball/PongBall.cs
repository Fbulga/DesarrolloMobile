using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PongBall : BaseBall
{
    private float speedIncreaseRatio;
    private float currentSpeed;
    private float maxSpeedFactor;

    protected override void Start()
    {
        base.Start();
        currentSpeed = ballData.Speed;
        speedIncreaseRatio = PongManager.Instance.SpeedIncreaseRatio;
        maxSpeedFactor = PongManager.Instance.MaxSpeedFactor;
    }



    protected override void CheckCollisions()
    {
        foreach (var collider in colliders)
        {
            if (collider == null) return;

            var response = collisionCheck.SphereRectangleCollisionStruct(collider, circleCollider2D);
            if (response.isTouching)
            {
                if (collider.TryGetComponent<ScoreZone>(out ScoreZone scoreZone))
                {
                    if (scoreZone.IsPlayer)
                    {
                        PongManager.Instance.OnIAScored?.Invoke();
                    }
                    ResetBall();
                    Debug.Log("Pong: Scored");
                    return;
                }

                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    if (currentSpeed < ballData.Speed * maxSpeedFactor)
                    {
                        currentSpeed *= 1f + (speedIncreaseRatio / 100f);
                    }
                }

                Reflect(response);
            }
        }
    }
    
    private void ResetBall()
    {
        currentSpeed = ballData.Speed;
        transform.position = PongManager.Instance.BallSpawn.transform.position;
        SetDirection(new Vector2(Random.Range(-1f, 1f), -1f));
    }
    
    protected override float GetSpeed() => currentSpeed;
    
    
}

