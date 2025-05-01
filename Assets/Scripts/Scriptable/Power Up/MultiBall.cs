using UnityEngine;

[CreateAssetMenu(fileName = "MultiBall", menuName = "PowerUp/MultiBall", order = 0)]
public class MultiBall : EffectSO
{
    
    [SerializeField] private float powerUpSpeed; 
    protected override float speed => powerUpSpeed;
    
    [SerializeField] GameObject ballPrefab;

    public override void Execute(GameObject gameObject)
    {
        if (ArkanoidManager.Instance.ActiveBalls.Count <= ArkanoidManager.Instance.ActiveBallsLimit)
        {
            foreach (GameObject ball in ArkanoidManager.Instance.ActiveBalls)
            {
                var newBall = Instantiate(ballPrefab, ball.transform.position, Quaternion.identity);
                newBall.GetComponent<ArkanoidBall>().SetDirection(ball.GetComponent<ArkanoidBall>().Direction * new Vector2(-1f,1f));
            }
        }
    }
}