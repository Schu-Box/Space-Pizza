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
    public GameObject tutorial_building;

    private float constructionDuration = 15f;
    private float constructionTimer = 0f;

    private bool timerStarted = false;
    public bool TimerStarted => timerStarted;
    
    private bool _buildTutorialShown = true;
    public bool BuildTutorialShown => _buildTutorialShown;
    
    private void Awake()
    {
        Instance = this;

        constructionTimer = constructionDuration;
        
        _buildTutorialShown = true;
        tutorial_building.SetActive(true);
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

    public void StartTimer()
    {
        timerStarted = true;
    }

    public void TimerFinished()
    {
        PhaseManager.Current.SwitchPhase(GamePhase.Fighting);
    }

    public void HideBuildTutorial()
    {
        _buildTutorialShown = false;
        tutorial_building.SetActive(false);
    }
}
