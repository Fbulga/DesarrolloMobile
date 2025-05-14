using System;
using Enum;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private string previousScene;
    private string currentScene;
    

    //Eventos
    public Action<string> OnChangeSceneRequested;
    public Action<GameManager> OnNewGameMode;
    public Action<int, string> OnGameOver;
    public Action OnResetGameMode;
    public Action OnMainMenu;
    public Action OnPlayAgain;
    public Action OnUploadJsonToCloud;
    
    public event Action OnCloudSyncSignInCompleted;
    
    
    [SerializeField] protected int playerScore;
    public int PlayerScore => playerScore;

    [SerializeField] private GameManager modeManager;


    private bool isMobilePlatform = false;
    public bool IsMobilePlatform => isMobilePlatform;
    
    
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
    }

    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            Input.compass.enabled = true;
            isMobilePlatform = true;
            if (SystemInfo.supportsGyroscope && Input.compass.enabled)
            {
                Debug.Log("El dispositivo tiene un magnetómetro (brújula disponible).");
            }
            else
            {
                Debug.Log("El dispositivo NO tiene un magnetómetro.");
            }
        }
        else
        {
            Debug.Log("NO HAY BRUJULA");
        }
        OnChangeSceneRequested += HandleChangeScene;
        OnNewGameMode += HandleNewGameMode;
        OnResetGameMode += HandleResetGameMode;
        OnMainMenu += HandleMainMenu;
        OnGameOver += HandleGameOver;
        OnPlayAgain += HandlePlayAgain;
        OnUploadJsonToCloud += HandleUploadJsonToCloud;
        OnCloudSyncSignInCompleted += HandleSingInCompleted;
    }


    private void HandleChangeScene(string sceneName)
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
        
    }
    
    private void HandleGameOver(int score, string reason)
    {
        playerScore = score;
        HandleChangeScene(reason);
    }
    private void HandleResetGameMode()
    {
        if (modeManager != null)
        {
            modeManager.ResetManager();
        }
    }
    
    private void HandleNewGameMode(GameManager gameModeManager)
    {
        modeManager = gameModeManager;
    }
    private void HandleMainMenu()
    {
        if (modeManager != null)
        {
            Destroy(modeManager.gameObject);
        }
        modeManager = null;
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.OnClearPool?.Invoke();
        }
    }
    private void HandlePlayAgain()
    { 
        HandleResetGameMode();
        if (modeManager != null)
        {
            Destroy(modeManager.gameObject);
        }
        HandleChangeScene(previousScene);
        StatManager.Instance.IncreaseStat(Stat.TotalMatchCount,1f);
        if(PoolManager.Instance != null)
        {
            PoolManager.Instance.OnClearPool?.Invoke();
        }
    }

    private void HandleSingInCompleted()
    {
        OnCloudSyncSignInCompleted?.Invoke();
    }

    private void HandleUploadJsonToCloud()
    {
        CloudSync.Instance.OnUploadJsonRequested?.Invoke();
    }
    protected virtual void ResetManager(){}
    protected virtual void UpdateScoreText(){}


}
