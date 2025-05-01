
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ArkanoidManager : GameManager
{
    public static ArkanoidManager Instance;

    public Action<GameObject> OnNewBall;
    public Action<GameObject> OnRemoveBall;
    public Action OnNewBrick;
    public Action<int> OnDestroyBrick;
    
    
    [SerializeField] private TextMeshProUGUI scoreText;
    private int bricksInGame = 0;
    private int ballsInGame = 0;
    
    
    private List<GameObject> activeBalls = new List<GameObject>();
    public List<GameObject> ActiveBalls => activeBalls;
    [SerializeField] private int activeBallsLimit;
    public int ActiveBallsLimit => activeBallsLimit;


    private DeadZone deadZone;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        
        GameManager.Instance.OnNewGameMode?.Invoke(this);
        OnNewBall += HandleBallInGame;
        OnRemoveBall += HandleRemoveBallInGame;
        OnNewBrick += HandleNewBrick;
        OnDestroyBrick += HandleDestroyBrick;
    }


    private void Start()
    {
        UpdateScoreText();
    }


    
    private void HandleBallInGame(GameObject ball)
    {
        ballsInGame++;
        if (!activeBalls.Contains(ball))
        {
            activeBalls.Add(ball);
        }
    }
    private void HandleRemoveBallInGame(GameObject ball)
    {
        ballsInGame--;
        if (activeBalls.Contains(ball))
        {
            activeBalls.Remove(ball);
            Destroy(ball);
        }
        
        if (ballsInGame <= 0)
        {
            Debug.Log("Game Over");
            
            GameManager.Instance.OnGameOver?.Invoke(playerScore,"Loss");
        }
    }
    
    private void HandleDestroyBrick(int points)
    {
        bricksInGame--;
        playerScore += points;
        UpdateScoreText();
        if (bricksInGame == 0)
        {
            Debug.Log("Win");
            GameManager.Instance.OnGameOver?.Invoke(playerScore,"Win");
        }
    }
    private void HandleNewBrick()
    {
        bricksInGame++;
    }

    
    public void SetDeadZone(DeadZone zone)
    {
        deadZone = zone;
    }
    public void DeactivateDeadZone(float duration)
    {
        deadZone.RunDeadlyTimer(duration);
    }


    protected override void ResetManager()
    {
        playerScore = 0;
        bricksInGame = 0;
        ballsInGame = 0;
        activeBalls.Clear();
    }
    protected override void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Points: "+ playerScore.ToString();
        }
        else
        {
            scoreText = GameObject.Find("PointsText").GetComponent<TextMeshProUGUI>();
        }
    }
}
