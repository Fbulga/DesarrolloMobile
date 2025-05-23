using System;
using Enum;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArkanoidBall : BaseBall
{
    protected override float GetSpeed() => ballData.Speed;

    void OnEnable()
    {
        ArkanoidManager.Instance.OnNewBall?.Invoke();
    }
    
    protected override void CheckCollisions()
    {
        foreach (var collider in colliders)
        {
            if (collider == null) return;

            var response = collisionCheck.SphereRectangleCollisionStruct(collider, circleCollider2D);
            if (response.isTouching)
            {
                if (collider.TryGetComponent<DeadZone>(out DeadZone deadZone) && deadZone.IsDeadly)
                {
                    DisableBall();
                }

                if (collider.TryGetComponent<IBreakable>(out IBreakable brick))
                {
                    brick.TryDestroyMe();
                }
                Reflect(response);
            }
        }
    }

    private void DisableBall()
    {
        ArkanoidManager.Instance.OnRemoveBall?.Invoke();
        VibrationManager.VibrateMedium(); 
        PoolManager.Instance.ReturnBall(gameObject,PrefabsType.ArkanoidBall);
    }
}