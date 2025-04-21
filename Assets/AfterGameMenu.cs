
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterGameMenu : MonoBehaviour
{
    public void PlayArkanoid()
    {
        ArkanoidGameManager.Instance.ResetManager();
        SceneManager.LoadScene("Arkanoid");
    }

    public void PlayPong()
    {
        PongGameManager.Instance.ResetManager();
        SceneManager.LoadScene("Pong");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
