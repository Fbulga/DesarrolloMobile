using System;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Serialization;


public class IAPaddleMovement : MonoBehaviour
{
    [SerializeField] private PaddleData paddleData;
    [SerializeField] private float reactionDistance;
    [SerializeField] public Transform ball;
    
    
    private float leftMovement;
    private float rightMovement;

    private Vector2 ballDirection;
    
    public float distance;
    [SerializeField] float treshold;
    [SerializeField] private float reactionCooldown;
    private float nextReactionTime;
    
    
    [SerializeField]public float aiDetection;
  
    public float direction;
    public float moveSpeedMultiplier;
    void Start()
    {
        leftMovement = -1f;
        rightMovement = 1f;
    }
    private void LateUpdate()
    {  

        CollisionRays();
        
        ballDirection = ball.GetComponent<PongBall>().Direction;
        if (ballDirection.y > 0f)
        {
            if(Time.time >= nextReactionTime )
            distance = ball.position.x - transform.position.x;
            float moveDir = Mathf.Sign(distance);
            if ((moveDir > 0 && rightMovement > 0f) || (moveDir < 0 && leftMovement < 0f))
            {
                float moveStep = -moveDir * paddleData.Speed * Time.deltaTime;
                if (Mathf.Abs(distance) > treshold)
                {
                    transform.Translate(0f, moveStep, 0f);    
                    nextReactionTime = Time.time + reactionCooldown;
                }
            }
        }
        
        
        /*
        ballDirection = ball.GetComponent<PongBall>().transform.position;
        
        if (Mathf.Abs(ballDirection.x - transform.position.x) > aiDetection)
        {
            direction = ballDirection.x > transform.position.x ? 1 : -1;
        }

        if ( rightMovement > 0f ||leftMovement < 0f)
        {
            direction = Mathf.Clamp(direction/50, leftMovement,rightMovement);
            
            transform.Translate(0f,-1f * paddleData.Speed * direction * moveSpeedMultiplier, 0f);
        }
        */
        
    }

    /*private void LateUpdate()
    {
        CollisionRays();
    }*/
    

    private void CollisionRays()
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, paddleData.RayDistance, paddleData.LimitLayerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, paddleData.RayDistance, paddleData.LimitLayerMask);
        
        
        if (hitRight.collider != null)
        {
            Debug.Log("toco el limite a la derecha: " + hitRight.collider.name);
            rightMovement = 0f;
        }
        else
        {
            rightMovement = 1f;
        }
        
        if (hitLeft.collider != null)
        {
            Debug.Log("toco el limite a la izquierda: " + hitLeft.collider.name);
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
}
