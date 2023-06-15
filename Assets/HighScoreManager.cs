using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;
    private int currentScore = 0;
    private bool highScoreAchieved = false;
    
    

    private void Awake()
    {
        Instance = this;
    }
    
    public void AddScore(int score)
    {
        if (GameplayInterfaceManager.Instance.IsGameOver)
        {
            return;
        }
        
        currentScore += score;

        GameplayInterfaceManager.Instance.UpdateScoreText(currentScore, highScoreAchieved);

        if (!highScoreAchieved && currentScore > PlayerPrefs.GetInt("highScore1"))
        {
            highScoreAchieved = true;
            GameplayInterfaceManager.Instance.DisplayHighScoreAchieved(currentScore);
        }
    }

    public void FinalizeScore()
    {
        PlayerPrefs.SetInt("highScore1", currentScore);
    }

    [ContextMenu("Clear Scores")]
    public void ClearScores()
    {
        PlayerPrefs.DeleteAll();
    }
}
