using System;
using Enum;
using UnityEngine;


    public class MainMenu:MonoBehaviour
    {
        private bool alreadyRequestedUpload = false;

        private void OnDisable()
        {
            GameManager.Instance.OnCloudSyncSignInCompleted -= UploadData;
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
    }
