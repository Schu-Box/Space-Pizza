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
    
    public int CurrentScore { get; private set; } = 0;
    
    public bool HighScoreAchieved { get; private set; } = false;

    private bool canScoreBeChanged = true;

    public void AddScore(int score)
    {
        if (!canScoreBeChanged)
        {
            return;
        }
        
        if (GameplayInterfaceManager.Instance.IsGameOver)
        {
            return;
        }
        
        CurrentScore += score;
        
        if (!HighScoreAchieved && CurrentScore > PlayerPrefs.GetInt("highScore1"))
        {
            HighScoreAchieved = true;
            GameplayInterfaceManager.Instance.UpdateScoreColor();
        }
        
        GameplayInterfaceManager.Instance.UpdateScoreText(CurrentScore, HighScoreAchieved);
    }

    public void FinalizeScore()
    {
        PlayerPrefs.SetInt("highScore1", CurrentScore);
    }

    [ContextMenu("Clear Scores")]
    public void ClearScores()
    {
        PlayerPrefs.DeleteAll();
    }

    public void LockScore()
    {
        canScoreBeChanged = false;
    }

    public void UnlockScore()
    {
        canScoreBeChanged = true;
    }
}
