using System;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Serialization;


public class IAPaddleMovement : MonoBehaviour
{
    [SerializeField] private PaddleData paddleData;
    [SerializeField] public GameObject ball;
    
    

    private float aimOffset;    
    private float leftMovement;
    private float rightMovement;
    private Vector2 ballDirection;
    [SerializeField] float threshold;
    [SerializeField] private float reactionDistance;
    [SerializeField] private float aimOffsetRange;    // cuanto puede desviarse el target
    [SerializeField] private float aiReaction; // entre 1 (facil) y 10 (dificl)
    
    void Start()
    {
        leftMovement = -1f;
        rightMovement = 1f;
        aiReaction = 5f;
    }
    private void Update()
    {
        CollisionRays();
        if (ball != null)
        {
            ballDirection = ball.GetComponent<BaseBall>().Direction;

            if (ballDirection.y > 0f)
            {
                float distanceX = ball.transform.position.x - transform.position.x;

                if (Mathf.Abs(distanceX) > threshold)
                {
                    float direction = Mathf.Sign(distanceX);

                    if ((direction > 0f && rightMovement > 0f) || (direction < 0f && leftMovement < 0f))
                    {
                        if (Mathf.Sign(aimOffset) != direction)
                        {
                            aimOffset = Random.Range(-aimOffsetRange, aimOffsetRange);
                        }

                        float moveStep = paddleData.Speed * Time.deltaTime;

                        Vector3 targetPosition = new Vector3(ball.transform.position.x + aimOffset,transform.position.y,transform.position.z);

                        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * aiReaction);
                    }
                }
            }
        }
    }

    public void IncreaseDifficulty()
    {
        if (aiReaction < 10f)
        {
            aiReaction++;
        }
    }
    
    public void DecreaseDifficulty()
    {
        if (aiReaction > 1f)
        {
            aiReaction--;
        }
    }
    
    private void CollisionRays()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, paddleData.RayDistance, paddleData.LimitLayerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, paddleData.RayDistance, paddleData.LimitLayerMask);
        
        
        if (hitRight.collider != null)
        {
            rightMovement = 0f;
        }
        else
        {
            rightMovement = 1f;
        }
        
        if (hitLeft.collider != null)
        {
            leftMovement = 0f;
        }
        else
        {
            leftMovement = -1f;
        }
        
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y) + Vector2.right * paddleData.RayDistance);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y) + Vector2.left * paddleData.RayDistance);
    }

    public void NewBall(GameObject newBall)
    {
       ball = newBall;
    }
}
