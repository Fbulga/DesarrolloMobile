using UnityEngine;


[CreateAssetMenu(fileName = "LongPaddle", menuName = "PowerUp/Long Paddle", order = 0)]
public class LongPaddle : EffectSO
{


    [SerializeField] private float powerUpSpeed; 
    protected override float speed => powerUpSpeed;
    
    [SerializeField]private float scaleMultiplier;
    [SerializeField]private float duration;
    
    public override void Execute(GameObject gameObject)
    {
        var paddle = gameObject.GetComponent<PaddleEffect>();
        if (paddle != null)
        {
            paddle.LongPaddle(scaleMultiplier, duration);
        }
    }
}
