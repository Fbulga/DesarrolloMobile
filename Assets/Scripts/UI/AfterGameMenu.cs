using Enum;
using TMPro;
using UnityEngine;
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
        GameManager.Instance.OnChangeSceneRequested?.Invoke("Menu");
    }

    private void UploadData()
    {
        GameManager.Instance.OnUploadJsonToCloud?.Invoke();
        alreadyRequestedUpload = true;
    }
    
    
}
