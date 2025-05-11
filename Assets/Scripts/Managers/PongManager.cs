using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;


public class PongManager : GameManager
{
    public static PongManager Instance;
    
    public Action OnIAScored;
    public Action OnPlayerScored;

    [SerializeField] private float speedIncreaseRatio;
    public float SpeedIncreaseRatio => speedIncreaseRatio;
    
    [SerializeField] private float maxSpeedFactor;
    public float MaxSpeedFactor => maxSpeedFactor;

    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI lifesText;
    [SerializeField] GameObject ballSpawn;
    public GameObject BallSpawn => ballSpawn;

    
    private float timer = 0f;
    [SerializeField] private int playerLifes;
    
    
    [SerializeField] private IAPaddleMovement aiPaddle;


    
    
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
        OnIAScored += HandleIAPoint;
        OnPlayerScored += HandlePlayerPoint;
    }

    private void Start()
    {
        UpdateLifesText();
        UpdateScoreText();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f && playerLifes > 0)
        {
            UpdateScoreText();
            playerScore++;
            timer -= 1f;
            Debug.Log("Contador: " + playerScore);
        }
    }
    
    private void HandleIAPoint()
    { 
        aiPaddle.DecreaseDifficulty();
        playerLifes--;
        UpdateLifesText();
        if (playerLifes <= 0)
        {
            GameManager.Instance.OnGameOver?.Invoke(playerScore,"PongSurvive");
        }
    }

    private void HandlePlayerPoint()
    {
        playerScore += 10;
        aiPaddle.IncreaseDifficulty();
        UpdateScoreText();
    }
    
    protected override void ResetManager()
    {
        playerScore = 0;
    }
    protected override void UpdateScoreText()
    {
        pointsText.text = $"Score: {playerScore}";
    }

    private void UpdateLifesText()
    {
        lifesText.text = $"Lifes left: {playerLifes}";
    }
    
}
