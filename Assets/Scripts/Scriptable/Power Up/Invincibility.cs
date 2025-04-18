using UnityEngine;

[CreateAssetMenu(fileName = "Invincibility", menuName = "PowerUp/Invincibility", order = 0)]
public class Invincibility : EffectSO
{
    [SerializeField]private float duration;
    
    public override void Execute(GameObject gameObject)
    {
        var paddle = gameObject.GetComponent<PaddleEffect>();
        if (paddle != null)
        {
            GameManager.Instance.DeactivateDeadZone(duration);
        }
    }
}
