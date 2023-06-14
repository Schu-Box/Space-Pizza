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

    private int coinCount = 0;

    private void Start()
    {
        Instance = this;
        
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
          Restart();
        }
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
    
    public void Restart()
    {
        Destroy(ShipManager.Current.PlayerShip.RootTransform.gameObject);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);  
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
