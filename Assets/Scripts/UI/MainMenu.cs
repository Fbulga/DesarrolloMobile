using UnityEngine;


    public class MainMenu:MonoBehaviour
    {
        public void PlayArkanoid()
        {
            VibrationManager.VibrateMedium();
            GameManager.Instance.OnChangeSceneRequested?.Invoke("Arkanoid");
        }

        public void PlayPong()
        {
            VibrationManager.VibrateMedium();
            GameManager.Instance.OnChangeSceneRequested?.Invoke("Pong");
        }
        public void Quit()
        {
            VibrationManager.VibrateMedium();
            Application.Quit();
        }
    }
