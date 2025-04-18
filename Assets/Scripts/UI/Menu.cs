using UnityEngine;
using UnityEngine.SceneManagement;

    public class Menu:MonoBehaviour
    {
        public void PlayArkanoid()
        {
            SceneManager.LoadScene("Arkanoid");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
