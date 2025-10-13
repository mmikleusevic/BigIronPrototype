using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeValueText;
    
    [Header("SFXVolume")]
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeValueText;
    
    [Space(20)]
    [SerializeField] private Button backButton;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        volumeSlider.onValueChanged.AddListener(VolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(SfxVolumeChanged);
        backButton.onClick.AddListener(BackToMainMenu);
    }
    
    private void OnDisable()
    {
        volumeSlider.onValueChanged.RemoveListener(VolumeChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(SfxVolumeChanged);
        backButton.onClick.RemoveListener(BackToMainMenu);
    }
    
    //TODO change volume 
    private void VolumeChanged(float value)
    {
        string text = GetVolumeText(value);
        volumeValueText.text = text;
    }
    
    //TODO change sfx volume 
    private void SfxVolumeChanged(float value)
    {
        string text = GetVolumeText(value);
        sfxVolumeValueText.text = text;
    }

    private void BackToMainMenu()
    {
        gameObject.SetActive(false);
    }
    
    private string GetVolumeText(float value)
    {
        return ((int)(value * 100)).ToString();
    }
}