using System;
using Enum;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArkanoidBall : BaseBall
{
    void OnEnable()
    {
        ArkanoidManager.Instance.OnNewBall?.Invoke();
    }
    
    protected override void HandleCollision(RaycastHit2D hit)
    {
        Collider2D collider = hit.collider;

        if (collider.TryGetComponent<DeadZone>(out DeadZone deadZone) && deadZone.IsDeadly)
        {
            DisableBall();
            return;
        }

        if (collider.TryGetComponent<IBreakable>(out IBreakable brick))
        {
            brick.TryDestroyMe();
        }
        
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2 newDir = CalculatePaddleBounce(hit.point, collider.transform);
            SetDirection(newDir);
            ballVisuals.HandleBounceEffect();
            return;
        }
        base.HandleCollision(hit);
    }

    private void DisableBall()
    {
        ArkanoidManager.Instance.OnRemoveBall?.Invoke();
        VibrationManager.VibrateMedium(); 
        PoolManager.Instance.ReturnBall(gameObject,PrefabsType.ArkanoidBall);
    }
}