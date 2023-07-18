using System;
using System.Collections;
using System.Collections.Generic;
using GamePhases;
using Helpers;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public struct HighScoreData
{
    public int rank;
    public string playerName;
    public int playerScore;
}

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Current => GameManager.Instance
        .ReferenceProvider.HighScoreManager;
    
    [SerializeField]
    private AnimationCurve pointsForLevelCompletion;
    
    public int CurrentScore { get; private set; } = 0;
    
    public bool HighScoreAchieved { get; private set; } = false;

    private string highScoreNamePrefix = "highScore_PlayerName";
    
    private string highScorePointsPrefix = "highScore_PlayerScore";
    
    private string highScoreRankPrefix = "highScore_PlayerRank";
    
    private bool canScoreBeChanged = true;

    private string BestStoredScore => $"{highScorePointsPrefix}0";

    private void Start()
    {
        ProgressManager.Current.CurrentLevelChangedEvent += HandleLevelCompleted;

        if (PlayerPrefs.GetInt($"{highScorePointsPrefix}{0}") == 0)
        {
            Debug.Log("Setting default scores!");
            SetDefaultScores();
        }
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
        
        if (!HighScoreAchieved && CurrentScore > PlayerPrefs.GetInt(BestStoredScore))
        {
            HighScoreAchieved = true;
            GameplayInterfaceManager.Instance.UpdateScoreColor();
        }
        
        GameplayInterfaceManager.Instance.UpdateScoreText(CurrentScore, HighScoreAchieved);
    }

    public List<HighScoreData> LoadTopFiveScores()
    {
        List<HighScoreData> topFiveData = new();
        
        for (int i = 0; i < 5; i++)
        {
            string playerNameKey = $"{highScoreNamePrefix}{i}";
            string rankKey = $"{highScoreRankPrefix}{i}";
            string pointsKey = $"{highScorePointsPrefix}{i}";

            if (!PlayerPrefs.HasKey(playerNameKey))
            {
                continue;
            }

            HighScoreData highScoreData = new();

            highScoreData.playerName = PlayerPrefs.GetString(playerNameKey);
            highScoreData.playerScore = PlayerPrefs.GetInt(pointsKey);
            highScoreData.rank = PlayerPrefs.GetInt(rankKey);
            
            topFiveData.Add(highScoreData);
        }

        return topFiveData;
    }

    public void FinalizeScore(string playerName)
    {
        List<HighScoreData> topFiveData = LoadTopFiveScores();

        HighScoreData currentPlayer = new HighScoreData
        {
            playerName = playerName,
            playerScore = CurrentScore,
            rank = 999,
        };
        
        topFiveData.Add(currentPlayer);
        
        // sort from highest to lowest
        topFiveData.Sort((data1, data2) => data1.playerScore.CompareTo(data2.playerScore));
        topFiveData.Reverse();

        for (int i = 0; i < 5; i++)
        {
            if (i >= topFiveData.Count)
            {
                break;
            }
            
            string playerNameKey = $"{highScoreNamePrefix}{i}";
            string rankKey = $"{highScoreRankPrefix}{i}";
            string pointsKey = $"{highScorePointsPrefix}{i}";

            HighScoreData currentData = topFiveData[i];
            
            PlayerPrefs.SetInt(rankKey, i + 1);
            PlayerPrefs.SetString(playerNameKey, currentData.playerName);
            PlayerPrefs.SetInt(pointsKey, currentData.playerScore);
        }
    }

    [ContextMenu("Clear Scores")]
    public void ClearScores()
    {
        PlayerPrefs.DeleteAll();
    }

    [ContextMenu("Set Default Scores")]
    public void SetDefaultScores()
    {
        ClearScores();
        
        string rankKey_1 = $"{highScoreRankPrefix}{0}";
        string playerNameKey_1 = $"{highScoreNamePrefix}{0}";
        string pointsKey_1 = $"{highScorePointsPrefix}{0}";
        PlayerPrefs.SetInt(rankKey_1, 1);
        PlayerPrefs.SetString(playerNameKey_1, "Charlie");
        PlayerPrefs.SetInt(pointsKey_1, 100000);
        
        string rankKey_2 = $"{highScoreRankPrefix}{1}";
        string playerNameKey_2 = $"{highScoreNamePrefix}{1}";
        string pointsKey_2 = $"{highScorePointsPrefix}{1}";
        PlayerPrefs.SetInt(rankKey_2, 2);
        PlayerPrefs.SetString(playerNameKey_2, "Paul");
        PlayerPrefs.SetInt(pointsKey_2, 50000);
        
        string rankKey_3 = $"{highScoreRankPrefix}{2}";
        string playerNameKey_3 = $"{highScoreNamePrefix}{2}";
        string pointsKey_3 = $"{highScorePointsPrefix}{2}";
        PlayerPrefs.SetInt(rankKey_3, 3);
        PlayerPrefs.SetString(playerNameKey_3, "Hassen");
        PlayerPrefs.SetInt(pointsKey_3, 20000);
        
        string rankKey_4 = $"{highScoreRankPrefix}{3}";
        string playerNameKey_4 = $"{highScoreNamePrefix}{3}";
        string pointsKey_4 = $"{highScorePointsPrefix}{3}";
        PlayerPrefs.SetInt(rankKey_4, 4);
        PlayerPrefs.SetString(playerNameKey_4, "Kirsten");
        PlayerPrefs.SetInt(pointsKey_4, 5000);
        
        string rankKey_5 = $"{highScoreRankPrefix}{4}";
        string playerNameKey_5 = $"{highScoreNamePrefix}{4}";
        string pointsKey_5 = $"{highScorePointsPrefix}{4}";
        PlayerPrefs.SetInt(rankKey_5, 5);
        PlayerPrefs.SetString(playerNameKey_5, "James");
        PlayerPrefs.SetInt(pointsKey_5, 2000);
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
