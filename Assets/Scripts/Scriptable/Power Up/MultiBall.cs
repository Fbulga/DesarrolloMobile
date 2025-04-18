using UnityEngine;

[CreateAssetMenu(fileName = "MultiBall", menuName = "PowerUp/MultiBall", order = 0)]
public class MultiBall : EffectSO
{
    private GameObject[] balls;
    public override void Execute(GameObject gameObject)
    {
        if (balls == null)
        {
            balls = GameObject.FindGameObjectsWithTag("Ball");
        }

        foreach (GameObject ball in balls)
        {
            Instantiate(ball, gameObject.transform.position, Quaternion.identity);
        }
    }
}