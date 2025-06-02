using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Enum;

public class ArkanoidManager : GameManager
{
    public static ArkanoidManager Instance;

    public Action OnNewBall;
    public Action OnRemoveBall;
    public Action OnNewBrick;
    public Action<int> OnDestroyBrick;
    
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject startText;
    private int bricksInGame = 0;
    private int ballsInGame = 0;
    
    [SerializeField] private GameObject canvas;
    [SerializeField] public GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;
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
        startText.SetActive(true);
    }
    
    private void HandleBallInGame()
    {
        ballsInGame++;
    }
    private void HandleRemoveBallInGame()
    {
        ballsInGame--;
        
        if (ballsInGame <= 0)
        {
            Debug.Log("Game Over");
            
            //GameManager.Instance.OnGameOver?.Invoke(playerScore,"Loss");
            GameManager.Instance.OnGameOver?.Invoke(playerScore,SceneIndex.Loss);
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
            //GameManager.Instance.OnGameOver?.Invoke(playerScore,"Win");
            
            GameManager.Instance.OnGameOver?.Invoke(playerScore,SceneIndex.ArkanoidWin);
            
            
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

    public void StartGame()
    {
        PoolManager.Instance.GetBall(PrefabsType.ArkanoidBall, spawnPoint.position);
        startText.SetActive(false);
    }
}
