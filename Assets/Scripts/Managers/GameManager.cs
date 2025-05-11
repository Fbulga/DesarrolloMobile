using System;
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
    
    
    [SerializeField] protected int playerScore;
    public int PlayerScore => playerScore;

    [SerializeField] private GameManager modeManager;
    
    
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
        OnChangeSceneRequested += HandleChangeScene;
        OnNewGameMode += HandleNewGameMode;
        OnResetGameMode += HandleResetGameMode;
        OnMainMenu += HandleMainMenu;
        OnGameOver += HandleGameOver;
        OnPlayAgain += HandlePlayAgain;
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
        Destroy(modeManager.gameObject);
        modeManager = null;
        if(PoolManager.Instance != null) PoolManager.Instance.OnClearPool?.Invoke();
    }
    private void HandlePlayAgain()
    { 
        HandleResetGameMode();
        Destroy(modeManager.gameObject);
        HandleChangeScene(previousScene);
        if(PoolManager.Instance != null)
        {
            PoolManager.Instance.OnClearPool?.Invoke();
        }
    }
    
    protected virtual void ResetManager(){}
    protected virtual void UpdateScoreText(){}
    
}
