using UnityEngine;
using System.Collections.Generic;
using Enum;

[CreateAssetMenu(fileName = "MultiBall", menuName = "PowerUp/MultiBall", order = 0)]
public class MultiBall : EffectSO
{
    [SerializeField] private float powerUpSpeed; 
    protected override float speed => powerUpSpeed;
    
    public override void Execute(GameObject gameObject)
    {
        var activeBalls = PoolManager.Instance.ActivePool[PrefabsType.ArkanoidBall];
        var ballsCopy = new List<GameObject>(activeBalls);

        foreach (GameObject ball in ballsCopy)
        {
            if (ball.activeInHierarchy)
            {
                if (PoolManager.Instance.ActivePool[PrefabsType.ArkanoidBall].Count <=
                    ArkanoidManager.Instance.ActiveBallsLimit)
                {
                    var newBall = PoolManager.Instance.GetBall(PrefabsType.ArkanoidBall,ball.transform.position);
                    newBall.GetComponent<BaseBall>().SetDirection(ball.GetComponent<BaseBall>().Direction * new Vector2(-1f, 1f));
                }
            }
        }
    }
}