using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MultiBall", menuName = "PowerUp/MultiBall", order = 0)]
public class MultiBall : EffectSO
{
    [SerializeField] private float powerUpSpeed; 
    protected override float speed => powerUpSpeed;
    
    public override void Execute(GameObject gameObject)
    {
        var activeBalls = ArkanoidPoolManager.Instance.ActivePool[ArkanoidManager.Instance.ballPrefab];
        var ballsCopy = new List<GameObject>(activeBalls);

        foreach (GameObject ball in ballsCopy)
        {
            if (ball.activeInHierarchy)
            {
                if (ArkanoidPoolManager.Instance.ActivePool[ArkanoidManager.Instance.ballPrefab].Count <=
                    ArkanoidManager.Instance.ActiveBallsLimit)
                {
                    var newBall = ArkanoidPoolManager.Instance.GetBall(ArkanoidManager.Instance.ballPrefab,ball.transform.position);
                    newBall.GetComponent<BaseBall>().SetDirection(ball.GetComponent<BaseBall>().Direction * new Vector2(-1f, 1f));
                }
            }
        }
    }
}