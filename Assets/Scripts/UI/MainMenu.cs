using UnityEngine;
using UnityEngine.SceneManagement;

    public class MainMenu:MonoBehaviour
    {
        public void PlayArkanoid()
        {
            SceneManager.LoadScene("Arkanoid");
        }

        public void PlayPong()
        {
            SceneManager.LoadScene("Pong");
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
