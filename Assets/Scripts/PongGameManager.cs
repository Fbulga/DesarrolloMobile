using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PongGameManager : MonoBehaviour, IManager
{
    public static PongGameManager Instance;
    
    public event Action OnPlayerScored;
    public event Action OnIAScored;

    
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] GameObject ballSpawn;

    public GameObject BallSpawn => ballSpawn;

    private float playerPoints;
    private float iAPoints;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnPlayerScored += PlayerPoint;
        OnIAScored += IAPoint;
        UpdateUI();
        
    }

    public void PlayerScored()
    {
        OnPlayerScored?.Invoke();
        Debug.Log("Player Scored");
    }

    public void IAScored()
    {
        OnIAScored?.Invoke();
        Debug.Log("IA Scored");
    }

    private void PlayerPoint()
    {
        playerPoints++;
        UpdateUI();
    }

    private void IAPoint()
    { 
        iAPoints++;
        UpdateUI();
    }


    private void UpdateUI()
    {
        pointsText.text = "Player: " + playerPoints.ToString() + " : " + iAPoints.ToString() + " :IA";

        if (iAPoints >= 3)
        {
            SceneManager.LoadScene("LossPong");
        }else if (playerPoints >= 3)
        {
            SceneManager.LoadScene("WinPong");
        }
    }

    public void ResetManager()
    {
        playerPoints = 0;
        iAPoints = 0;
    }
}
