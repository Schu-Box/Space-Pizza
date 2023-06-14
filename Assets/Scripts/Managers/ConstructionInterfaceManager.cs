using System.Collections;
using System.Collections.Generic;
using GamePhases;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionInterfaceManager : MonoBehaviour
{
    public static ConstructionInterfaceManager Instance;

    public TextMeshProUGUI countdownText;
    public Slider countdownSlider;

    private float constructionDuration = 15f;
    private float constructionTimer = 0f;

    private bool timerStarted = true;
    
    private void Start()
    {
        Instance = this;

        constructionTimer = constructionDuration;
    }

    void Update()
    {
        if (timerStarted)
        {
            constructionTimer -= Time.deltaTime;

            countdownSlider.value = constructionTimer / constructionDuration;

            if (constructionTimer <= 0f)
            {
                TimerFinished();
            }
        }
    }

    public void TimerFinished()
    {
        Debug.Log("time's up!");
        PhaseManager.Current.SwitchPhase(GamePhase.Fighting);
    }
}
