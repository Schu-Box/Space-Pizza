using System;
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
    
    public GameObject jumpDriveAnimation;

    public TextMeshProUGUI jumpDriveChargeText;
    public Slider jumpDriveChargeSlider;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI highScoreText;

    [SerializeField]
    private TMP_Text levelTextField;

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

    private void Start()
    {
        UpdateScoreColor();
        UpdateScoreText(HighScoreManager.Current.CurrentScore, 
            HighScoreManager.Current.HighScoreAchieved);

        levelTextField.text = $"Level: {ProgressManager.Current.CurrentLevel + 1}";
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
        // no more scoring past this point
        HighScoreManager.Current.LockScore();
        
        jumpDriveChargeText.text = "Jump Drive Ready! Press Space!";
    }

    public void DisplayGameOver()
    {
        _gameOver = true;

        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over";

        HighScoreManager.Current.FinalizeScore();
    }

    [ContextMenu("ActivateJumpDrive")]
    public void ActivateJumpDrive()
    {
        StartCoroutine(RunJumpSequence());
    }

    private IEnumerator RunJumpSequence()
    {
        PhaseManager.Current.StartJump();

        jumpEffect.StartEffect(jumpEffectDuration);
        
        Ship ship = ShipManager.Current.PlayerShip;
        
        Vector3 jumpPositioning = ship.RootTransform.position;

        yield return new WaitForSecondsRealtime(jumpEffectDuration * 0.25f);

        Instantiate(jumpDriveAnimation, jumpPositioning, Quaternion.identity);
        
        yield return new WaitForSeconds(jumpEffectDuration * 0.75f + 0.5f);

        GameManager.Instance.ReferenceProvider.PhaseManager.SwitchPhase(GamePhase.Construction);

        ship.RootTransform.position = ShipGridController.Current.CorePosition.GridPosition();
        ship.RootTransform.eulerAngles = Vector3.zero;
        ship.StopPhysics();
                
        ship.ChangeVisibility(true);
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

    public void UpdateScoreColor()
    {
        if (HighScoreManager.Current.HighScoreAchieved)
        {
            highScoreText.color = Color.yellow;
            return;
        }

        highScoreText.color = Color.white;
    }
}