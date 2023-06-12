using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public TextMeshProUGUI jumpDriveChargeText;
    public Slider jumpDriveChargeSlider;

    public float jumpDriveChargeDuration = 30f;
    private float jumpDriveChargeTimer = 0f;

    private bool jumpDriveReady = false;

    // Update is called once per frame
    void Update()
    {
        if (!jumpDriveReady)
        { jumpDriveChargeTimer += Time.deltaTime;

            jumpDriveChargeSlider.value = jumpDriveChargeTimer / jumpDriveChargeDuration;
        
            if (jumpDriveChargeTimer >= jumpDriveChargeDuration)
            {
                JumpDriveReady();
            }
        }
    }

    private void JumpDriveReady()
    {
        jumpDriveReady = true;
        jumpDriveChargeText.text = "Jump Drive Ready!";
    }
}
