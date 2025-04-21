using UnityEngine;

[CreateAssetMenu(fileName = "MultiBall", menuName = "PowerUp/MultiBall", order = 0)]
public class MultiBall : EffectSO
{
    
    [SerializeField] private float powerUpSpeed; 
    protected override float speed => powerUpSpeed;
    
    
    [SerializeField] GameObject ballPrefab;
    public override void Execute(GameObject gameObject)
    {
        if (GameManager.Instance.ActiveBalls.Count <= GameManager.Instance.ActiveBallsLimit)
        {
            foreach (GameObject ball in GameManager.Instance.ActiveBalls)
            {
                var newBall = Instantiate(ballPrefab, ball.transform.position, Quaternion.identity);
                newBall.GetComponent<Ball>().SetDirection(ball.GetComponent<Ball>().Direction * new Vector2(-1f,1f));
            }
        }
    }
}