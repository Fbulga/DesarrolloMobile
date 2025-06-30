using System;
using System.Collections;
using Enum;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private int previousScene;
    private string currentScene;
    

    //Eventos
    public Action<SceneIndex> OnChangeSceneRequested;
    public Action<GameManager> OnNewGameMode;
    public Action<int, SceneIndex> OnGameOver;
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
    
    
    
    
    //Loading Scene
    public GameObject loadingScreen;
    public Slider slider;
    [SerializeField] private float fillSpeed; 
    
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


    private void HandleChangeScene(SceneIndex sceneIndex)
    {
        previousScene = SceneManager.GetActiveScene().buildIndex;
      
        slider.value = 0f; 
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync((int)sceneIndex));
        PoolManager.Instance.OnClearPool?.Invoke();
    }
    
    private void HandleGameOver(int score, SceneIndex reason)
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
        HandleChangeScene((SceneIndex)previousScene);
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


    
    private IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        operation.allowSceneActivation = false; 

        while (!operation.isDone)
        {
           
            float targetFill = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = Mathf.MoveTowards(slider.value, targetFill, fillSpeed * Time.deltaTime);;
            
            if (operation.progress >= 0.9f && slider.value >= 0.999f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    
    
}
