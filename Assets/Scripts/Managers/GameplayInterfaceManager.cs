using System.Collections;
using Effects;
using GamePhases;
using Helpers;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameplayInterfaceManager : MonoBehaviour
{
    public static GameplayInterfaceManager Instance;

    [SerializeField]
    private JumpEffect jumpEffect;

    [SerializeField]
    private float jumpEffectDuration = 5f;

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

    [ContextMenu("ActivateJumpDrive")]
    public void ActivateJumpDrive()
    {
        StartCoroutine(RunJumpSequence());
    }

    private IEnumerator RunJumpSequence()
    {
        PhaseManager.Current.ChangeJumpState(true);
        
        jumpEffect.StartEffect(jumpEffectDuration);

        yield return new WaitForSecondsRealtime(jumpEffectDuration + 0.5f);
        
        PhaseManager.Current.ChangeJumpState(false);
        
        GameManager.Instance.ReferenceProvider.PhaseManager.SwitchPhase(GamePhase.Construction);
        
        Ship ship = ShipManager.Current.PlayerShip;
        ship.ChangeVisibility(true);

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