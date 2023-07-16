using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer audioMixer;

    private string musicVolumeKey = "MusicVolume";
    private string sfxVolumeKey = "SFXVolume";
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetVolumes();
    }

    private void SetVolumes()
    {
        float musicVolume = PlayerPrefs.GetFloat(musicVolumeKey, 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat(sfxVolumeKey, 0.5f);

        float convertedMusicValue = Mathf.Log10(musicVolume) * 20f;
        float convertedSFXValue = Mathf.Log10(sfxVolume) * 20f;
        
        audioMixer.SetFloat(musicVolumeKey, convertedMusicValue);
        audioMixer.SetFloat(sfxVolumeKey, convertedSFXValue);
    }
    
    public void ChangeMusicVolume(float sliderValue)
    {
       float convertedValue = Mathf.Log10(sliderValue) * 20f;
        
        audioMixer.SetFloat(musicVolumeKey, convertedValue);
        PlayerPrefs.SetFloat(musicVolumeKey, sliderValue);
    }

    public void ChangeSFXVolume(float sliderValue)
    {
        float convertedValue = Mathf.Log10(sliderValue) * 20f;
        
        audioMixer.SetFloat(sfxVolumeKey, convertedValue);
        PlayerPrefs.SetFloat(sfxVolumeKey, sliderValue);
    }
}
