using UnityEngine;


    public class MainMenu:MonoBehaviour
    {
        public void PlayArkanoid()
        {
            GameManager.Instance.OnChangeSceneRequested?.Invoke("Arkanoid");
        }

        public void PlayPong()
        {
            GameManager.Instance.OnChangeSceneRequested?.Invoke("Pong");
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
