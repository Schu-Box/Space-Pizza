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
    public TextMeshProUGUI coinCountText;

    public GameObject tutorial_WASD;

    private bool wASDTutorialShown = true;
    public bool WASDTutorialShown => wASDTutorialShown;

    private int coinCount = 0;

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
        coinCount += coins;
        coinCountText.text = "Coins: " + coinCount;
    }

    public void DisplayJumpDriveReady()
    {
        jumpDriveChargeText.text = "Jump Drive Ready! Press Space!";
    }

    public void DisplayGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over";
    }

    public void DisplaySuccess()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Success!";
    }

    public void ActivateJumpDrive()
    {
        // Destroy(GameManager.Instance.gameObject);
        // SceneManager.LoadScene(0);
        GameManager.Instance.ReferenceProvider.PhaseManager.SwitchPhase(GamePhase.Construction);

        Ship ship = ShipManager.Current.PlayerShip;
        ship.RootTransform.position = ShipGridController.Current.CorePosition;
        ship.RootTransform.eulerAngles = Vector3.zero;
        ship.StopPhysics();
    }
}
