using System;
using System.Collections;
using System.Collections.Generic;
using GamePhases;
using Managers;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Current => GameManager.Instance
        .ReferenceProvider.HighScoreManager;
    
    private int currentScore = 0;
    private bool highScoreAchieved = false;

    public bool HighScoreAchieved => highScoreAchieved;

    public void AddScore(int score)
    {
        if (GameplayInterfaceManager.Instance.IsGameOver)
        {
            return;
        }
        
        currentScore += score;
        
        if (!highScoreAchieved && currentScore > PlayerPrefs.GetInt("highScore1"))
        {
            highScoreAchieved = true;
            GameplayInterfaceManager.Instance.UpdateScoreColor();
        }
        
        GameplayInterfaceManager.Instance.UpdateScoreText(currentScore, highScoreAchieved);
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
