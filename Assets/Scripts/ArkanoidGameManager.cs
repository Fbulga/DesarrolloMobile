using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArkanoidGameManager : MonoBehaviour, IManager
{
    public static ArkanoidGameManager Instance;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private int totalPoints = 0;
    private int bricksInGame = 0;
    private int ballsInGame = 0;
    
    private List<GameObject> activeBalls = new List<GameObject>();
    public List<GameObject> ActiveBalls => activeBalls;
    [SerializeField] private int activeBallsLimit;
    public int ActiveBallsLimit => activeBallsLimit;


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

        //DontDestroyOnLoad(gameObject);
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
        if (scoreText != null)
        {
            scoreText.text = "Points: "+ totalPoints.ToString();
        }
        else
        {
            scoreText = GameObject.Find("PointsText").GetComponent<TextMeshProUGUI>();
        }
    }

    
    //Ball
    private void AddBallInGame(GameObject ball)
    {
        ballsInGame++;
        if (!activeBalls.Contains(ball))
        {
            activeBalls.Add(ball);
        }
    }
    private void SubtractBallInGame(GameObject ball)
    {
        ballsInGame--;
        if (activeBalls.Contains(ball))
        {
            activeBalls.Remove(ball);
        }
        
        if (ballsInGame <= 0)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("LossArkanoid");
        }
    }
    public void NewBallInGame(GameObject ball)
    {
        AddBallInGame(ball);
    }
    public void RemoveBall(GameObject ball)
    {
        SubtractBallInGame(ball);
    }

    
    //Brick
    private void SubtractBrickInGame()
    {
        bricksInGame--;
        if (bricksInGame == 0)
        {
            Debug.Log("Win");
            SceneManager.LoadScene("WinArkanoid");
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


    public void ResetManager()
    {
        totalPoints = 0;
        bricksInGame = 0;
        ballsInGame = 0;
        activeBalls.Clear();
    }
}
