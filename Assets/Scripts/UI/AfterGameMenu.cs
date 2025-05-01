using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterGameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: "+ GameManager.Instance.PlayerScore.ToString();
        }   
    }
    
    public void PlayAgain()
    {
        GameManager.Instance.OnPlayAgain?.Invoke();
        GameManager.Instance.OnResetGameMode?.Invoke();
    }
    public void MainMenu()
    {
        Debug.Log("MenuPressed");
        GameManager.Instance.OnMainMenu?.Invoke();
        GameManager.Instance.OnChangeSceneRequested?.Invoke("Menu");
    }

    
    
}
