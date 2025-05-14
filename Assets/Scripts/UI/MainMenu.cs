using System;
using Enum;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu:MonoBehaviour
    {
        private bool alreadyRequestedUpload = false;

        [SerializeField] private GameObject statsPanel;

        private void OnDisable()
        {
            GameManager.Instance.OnCloudSyncSignInCompleted -= UploadData;
        }

        private void Awake()
        {
            CanvasScaler canvasScaler = gameObject.GetComponent<CanvasScaler>();
            if(Application.isMobilePlatform){
                canvasScaler.scaleFactor = 1f;
            }
            else
            {
                canvasScaler.scaleFactor = 0.5f;
            }
        }
        private void Start()
        {
            GameManager.Instance.OnCloudSyncSignInCompleted += UploadData;
            if (!alreadyRequestedUpload)
            {
                UploadData();
            }
        }

        public void PlayArkanoid()
        {
            VibrationManager.VibrateMedium();
            GameManager.Instance.OnChangeSceneRequested?.Invoke("Arkanoid");
            StatManager.Instance.IncreaseStat(Stat.TotalMatchCount,1f);
        }

        public void PlayPong()
        {
            VibrationManager.VibrateMedium();
            GameManager.Instance.OnChangeSceneRequested?.Invoke("Pong");
            StatManager.Instance.IncreaseStat(Stat.TotalMatchCount, 1f);
        }
        public void Quit()
        {
            VibrationManager.VibrateMedium();
            Application.Quit();
        }

        private void UploadData()
        {
           GameManager.Instance.OnUploadJsonToCloud?.Invoke();
           alreadyRequestedUpload = true;
        }


        public void Stats()
        {
            VibrationManager.VibrateMedium();
            statsPanel.SetActive(true);
            if(!alreadyRequestedUpload){
                UploadData();
            }
        }

        public void Back(){
            VibrationManager.VibrateMedium();
            statsPanel.SetActive(false);
        }
    }
