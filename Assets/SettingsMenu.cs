using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu Instance;
    
    public GameObject settingsMenuPanel;
    
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }    
    }

    private bool paused = false;
    public void ToggleSettingsMenu()
    {
        paused = !paused;
        
        settingsMenuPanel.SetActive(paused);
        
        if (paused)
        {
            SetSliderValues();
            
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void SetSliderValues()
    {
        float musicVolumeValue = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        musicVolumeSlider.value = musicVolumeValue;
        
        float sfxVolumeValue = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        sfxVolumeSlider.value = sfxVolumeValue;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        paused = false;
        
        Destroy(ShipManager.Current.PlayerShip.RootTransform.gameObject);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);  
    }
}
