using System;
using System.Collections;
using System.Collections.Generic;
using GamePhases;
using Helpers;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Current => GameManager.Instance
        .ReferenceProvider.HighScoreManager;
    
    [SerializeField]
    private AnimationCurve pointsForLevelCompletion;
    
    public int CurrentScore { get; private set; } = 0;
    
    public bool HighScoreAchieved { get; private set; } = false;

    private bool canScoreBeChanged = true;

    private void Start()
    {
        ProgressManager.Current.CurrentLevelChangedEvent += HandleLevelCompleted;
    }
    
    private void OnDestroy()
    {
        ProgressManager.Current.CurrentLevelChangedEvent -= HandleLevelCompleted;
    }
    
    private void HandleLevelCompleted()
    {
        // the completed level is the previous one
        int completedLevel = ProgressManager.Current.CurrentLevel - 1;

        int pointsForCompletion = Mathf.RoundToInt(pointsForLevelCompletion.EvaluateLimitless(completedLevel));
        
        AddScore(pointsForCompletion, overwriteScoreLock: true);
    }

    public void AddScore(int score, bool overwriteScoreLock = false)
    {
        if (!canScoreBeChanged && !overwriteScoreLock)
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
