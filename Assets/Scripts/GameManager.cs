using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] TextMeshProUGUI scoreText;
    private int totalPoints = 0;
    private int ballsInGame = 0;
    private int bricksInGame = 0;
    
    private DeadZone deadZone;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        UpdateScoreText();
    }

    //Score
    private void AddTotalPoints(int points)
    {
        totalPoints += points;
        UpdateScoreText();
    }
    public void AddPoints(int points)
    {
        AddTotalPoints(points);
    }
    private void UpdateScoreText()
    {
        scoreText.text = totalPoints.ToString();
    }

    
    //Ball
    private void AddBallInGame()
    {
        ballsInGame++;
    }
    private void SubtractBallInGame()
    {
        ballsInGame--;
        if (ballsInGame <= 0)
        {
            Debug.Log("Game Over");
        }
    }
    public void NewBallInGame()
    {
        AddBallInGame();
    }
    public void RemoveBall()
    {
        SubtractBallInGame();
    }

    
    //Brick
    private void SubtractBrickInGame()
    {
        bricksInGame--;
        if (bricksInGame == 0)
        {
            Debug.Log("Win");
        }
    }
    private void AddBrickInGame()
    {
        bricksInGame++;
    }
    public void NewBrickInGame()
    {
        AddBrickInGame();
    }
    public void DestroyBrick(int points)
    {
        SubtractBrickInGame();
        AddTotalPoints(points);
    }


    
    
    //DeadZone PowerUp
    public void SetDeadZone(DeadZone zone)
    {
        deadZone = zone;
    }
    public void DeactivateDeadZone(float duration)
    {
        deadZone.RunDeadlyTimer(duration);
    }
}
