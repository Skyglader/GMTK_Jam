using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static float savedMusicVolume = 2;
    public static float savedSFXVolume = 2;

    public static SoundSettings instance;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        SetMusicVolume(savedMusicVolume);
        musicSlider.value = savedMusicVolume;
        SetSFXVolume(savedSFXVolume);
        sfxSlider.value = savedSFXVolume;  
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        savedMusicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
        savedSFXVolume = volume;
    }
}
