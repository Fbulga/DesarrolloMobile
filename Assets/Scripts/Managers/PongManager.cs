using System;
using UnityEngine;
using TMPro;


public class PongManager : GameManager
{
    public static PongManager Instance;
    
    public Action OnPlayerScored;
    public Action OnIAScored;

    [SerializeField] private float speedIncreaseRatio;
    public float SpeedIncreaseRatio => speedIncreaseRatio;
    
    [SerializeField] private float maxSpeedFactor;
    public float MaxSpeedFactor => maxSpeedFactor;

    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] GameObject ballSpawn;

    
    
    public GameObject BallSpawn => ballSpawn;
    
    private float iAPoints;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        GameManager.Instance.OnNewGameMode?.Invoke(this);
        OnPlayerScored += HandlePlayerPoint;
        OnIAScored += HandleIAPoint;
    }

    private void Start()
    {

        UpdateScoreText();
    }
    
    
    private void HandlePlayerPoint()
    {
        playerScore++;
        UpdateScoreText();
    }
    private void HandleIAPoint()
    { 
        iAPoints++;
        UpdateScoreText();
    }
    protected override void ResetManager()
    {
        playerScore = 0;
        iAPoints = 0;
    }
    protected override void UpdateScoreText()
    {
        pointsText.text = "Player: " + playerScore.ToString() + " : " + iAPoints.ToString() + " :IA";

        if (iAPoints >= 3)
        {
            GameManager.Instance.OnGameOver?.Invoke(playerScore,"Loss");
        }
        else if (playerScore >= 3)
        {
            GameManager.Instance.OnGameOver?.Invoke(playerScore,"Win");
        }
    }
}
