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
    public Action<float, string> OnGameOver;
    public Action OnResetGameMode;
    public Action OnMainMenu;
    public Action OnPlayAgain;
    
    
    [SerializeField] protected float playerScore;
    public float PlayerScore => playerScore;

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
        Debug.Log(previousScene);
    }
    
    private void HandleGameOver(float score, string reason)
    {
        playerScore = score;
        HandleChangeScene(reason);
    }
    private void HandleResetGameMode()
    {
        if (modeManager != null)
        {
            Debug.Log("Reset Game Mode");
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
    }
    private void HandlePlayAgain()
    { 
        HandleResetGameMode();
        Destroy(modeManager.gameObject);
        Debug.Log(previousScene);
        HandleChangeScene(previousScene);
    }
    
    protected virtual void ResetManager(){}
    protected virtual void UpdateScoreText(){}
    
}
