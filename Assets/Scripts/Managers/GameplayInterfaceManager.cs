using System.Collections;
using System.Collections.Generic;
using GamePhases;
using Helpers;
using Managers;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayInterfaceManager : MonoBehaviour
{
    public static GameplayInterfaceManager Instance;

    public TextMeshProUGUI jumpDriveChargeText;
    public Slider jumpDriveChargeSlider;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI highScoreText;

    public GameObject tutorial_WASD;

    private bool wASDTutorialShown = true;
    public bool WASDTutorialShown => wASDTutorialShown;

    private int coinCount = 0;

    private bool _gameOver = false;
    public bool IsGameOver => _gameOver;

    private void Awake()
    {
        Instance = this;

        gameOverText.gameObject.SetActive(false);

        wASDTutorialShown = true;
        tutorial_WASD.SetActive(true);
    }

    void Update()
    {

    }

    public void HideWASDTutorial()
    {
        wASDTutorialShown = false;
        tutorial_WASD.SetActive(false);
    }

    public void UpdateJumpDriveSlider(float value)
    {
        jumpDriveChargeSlider.value = value;
    }

    public void AddCoin(int coins)
    {
        // coinCount += coins;
        // coinCountText.text = "Coins: " + coinCount;
    }

    public void DisplayJumpDriveReady()
    {
        jumpDriveChargeText.text = "Jump Drive Ready! Press Space!";
    }

    public void DisplayGameOver()
    {
        _gameOver = true;
        
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over";

        HighScoreManager.Instance.FinalizeScore();
    }

    public void ActivateJumpDrive()
    {
        // Destroy(GameManager.Instance.gameObject);
        // SceneManager.LoadScene(0);
        GameManager.Instance.ReferenceProvider.PhaseManager.SwitchPhase(GamePhase.Construction);

        Ship ship = ShipManager.Current.PlayerShip;
        ship.RootTransform.position = ShipGridController.Current.CorePosition.GridPosition();
        ship.RootTransform.eulerAngles = Vector3.zero;
        ship.StopPhysics();
    }

    public void UpdateScoreText(int newScore, bool isHighScore = false)
    {
        if (isHighScore)
        {
            highScoreText.text = "High Score: " + newScore;
        }
        else
        {
            highScoreText.text = "Score: " + newScore;
        }
    }

    public void DisplayHighScoreAchieved(int newScore)
    {
        highScoreText.color = Color.yellow;
    }
}
