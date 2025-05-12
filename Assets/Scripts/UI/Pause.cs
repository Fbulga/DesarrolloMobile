using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    
    private bool alreadyRequestedUpload = false;
    private void OnEnable()
    {
        GameManager.Instance.OnCloudSyncSignInCompleted += UploadData;
        if (!alreadyRequestedUpload)
        {
            UploadData();
        }
        alreadyRequestedUpload = true;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCloudSyncSignInCompleted -= UploadData;
        alreadyRequestedUpload = false;
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    
    public void MainMenu()
    {
        Debug.Log("MenuPressed");
        Time.timeScale = 1;
        GameManager.Instance.OnMainMenu?.Invoke();
        GameManager.Instance.OnChangeSceneRequested?.Invoke("Menu");
    }
    
    private void UploadData()
    {
        GameManager.Instance.OnUploadJsonToCloud?.Invoke();
        alreadyRequestedUpload = true;
    }
}
