using System.Collections;
using System.Collections.Generic;
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

    private float jumpDriveChargeDuration = 90f;
    private float jumpDriveChargeTimer = 0f;

    private bool jumpDriveReady = false;
    private int coinCount = 0;

    private void Start()
    {
        Instance = this;
        
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!jumpDriveReady)
        { 
            jumpDriveChargeTimer += Time.deltaTime;

            jumpDriveChargeSlider.value = jumpDriveChargeTimer / jumpDriveChargeDuration;
        
            if (jumpDriveChargeTimer >= jumpDriveChargeDuration)
            {
                JumpDriveReady();
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            Destroy(ShipManager.Current.PlayerShip.gameObject);
            SceneManager.LoadScene(0);
        }
    }

    public void AddCoin(int coins)
    {
        coinCount += coins;
        coinCountText.text = "Coins: " + coinCount;
    }

    private void JumpDriveReady()
    {
        jumpDriveReady = true;
        jumpDriveChargeText.text = "Jump Drive Ready!";
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
}
