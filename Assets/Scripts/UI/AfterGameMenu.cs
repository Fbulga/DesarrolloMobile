using Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AfterGameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private bool alreadyRequestedUpload = false;
    private void OnEnable()
    {
        GameManager.Instance.OnCloudSyncSignInCompleted += UploadData;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCloudSyncSignInCompleted -= UploadData;
    }
    
    
    private void Start()
    {
        StatManager.Instance.IncreaseStat(Stat.TotalScore,GameManager.Instance.PlayerScore);
        if (scoreText != null)
        {
            scoreText.text = $"Score: {GameManager.Instance.PlayerScore}";
        }  
        if (!alreadyRequestedUpload)
        {
            UploadData();
        }
        
        CanvasScaler canvasScaler = gameObject.GetComponent<CanvasScaler>();
        if(GameManager.Instance.IsMobilePlatform){
            canvasScaler.scaleFactor = 1f;
        }else{
            canvasScaler.scaleFactor = 0.5f;
        }
    }
    
    public void PlayAgain()
    {
        VibrationManager.VibrateMedium();
        GameManager.Instance.OnPlayAgain?.Invoke();
        GameManager.Instance.OnResetGameMode?.Invoke();
    }
    public void MainMenu()
    {
        Debug.Log("MenuPressed");
        VibrationManager.VibrateMedium();
        GameManager.Instance.OnMainMenu?.Invoke();
        //GameManager.Instance.OnChangeSceneRequested?.Invoke("Menu");
        GameManager.Instance.OnChangeSceneRequested?.Invoke(SceneIndex.MainMenu);
    }

    private void UploadData()
    {
        GameManager.Instance.OnUploadJsonToCloud?.Invoke();
        alreadyRequestedUpload = true;
    }
    
    
}
