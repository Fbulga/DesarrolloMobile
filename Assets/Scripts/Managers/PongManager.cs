using Enum;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;



public class PongManager : GameManager
{
    public static PongManager Instance;
    
    public Action OnIAScored;
    public Action OnPlayerScored;

    [SerializeField] private float speedIncreaseRatio;
    public float SpeedIncreaseRatio => speedIncreaseRatio;
    
    [SerializeField] private float maxSpeedFactor;
    public float MaxSpeedFactor => maxSpeedFactor;

    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI lifesText;
    [SerializeField] private Transform ballSpawn;
    [SerializeField] private GameObject canvas;
    public Transform BallSpawn => ballSpawn;
    
    [SerializeField] private GameObject startText;

    
    private float timer = 0f;
    [SerializeField] private int playerLifes;
    
    
    [SerializeField] private IAPaddleMovement aiPaddle;
    public IAPaddleMovement AIPaddle => aiPaddle;

    private bool matchStart = false;
    
    
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
        startText.SetActive(true);
        /*
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        if(GameManager.Instance.IsMobilePlatform){
            canvasScaler.scaleFactor = 1f;
        }else{
            canvasScaler.scaleFactor = 0.5f;
        }
        */
    }

    private void Update()
    {
        if (matchStart)
        {
            timer += Time.deltaTime;

            if (timer >= 1f && playerLifes > 0)
            {
                UpdateScoreText();
                playerScore++;
                timer -= 1f;
            }
        }
        
    }
    
    private void HandleIAPoint()
    { 
        aiPaddle.DecreaseDifficulty();
        playerLifes--;
        UpdateLifesText();
        StatManager.Instance.IncreaseStat(Stat.IAGoals,1f);
        if (playerLifes <= 0)
        {
            //GameManager.Instance.OnGameOver?.Invoke(playerScore,"PongSurvive");
            GameManager.Instance.OnGameOver?.Invoke(playerScore,SceneIndex.PongWin);
        }
    }

    private void HandlePlayerPoint()
    {
        playerScore += 10;
        aiPaddle.IncreaseDifficulty();
        UpdateScoreText();
        StatManager.Instance.IncreaseStat(Stat.PlayerGoals,1f);
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
    
    public void StartGame()
    {
        PoolManager.Instance.GetBall(PrefabsType.PongBall, ballSpawn.position);
        startText.SetActive(false);
        matchStart = true;
    }
}
