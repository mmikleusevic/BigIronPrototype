using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionsUI : MonoBehaviour
    {
        [Header("Volume")]
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TextMeshProUGUI volumeValueText;
    
        [Header("SFXVolume")]
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private TextMeshProUGUI sfxVolumeValueText;        
        
        [Header("AimSensitivity")]
        [SerializeField] private Slider aimSensitivitySlider;
        [SerializeField] private TextMeshProUGUI aimSensitivityValueText;
    
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
            aimSensitivitySlider.onValueChanged.AddListener(AimSensitivityChanged);
            backButton.onClick.AddListener(BackToMainMenu);
        }
    
        private void OnDisable()
        {
            volumeSlider.onValueChanged.RemoveListener(VolumeChanged);
            sfxVolumeSlider.onValueChanged.RemoveListener(SfxVolumeChanged);
            aimSensitivitySlider.onValueChanged.RemoveListener(AimSensitivityChanged);
            backButton.onClick.RemoveListener(BackToMainMenu);
        }

        private void Start()
        {
            float volume = 0;
            float sfxVolume = 0;
            float aimSensitivity = 0;

            if (!SoundManager.Instance) return;
            
            SoundManager.Instance.GetVolume();
            volumeSlider.value = volume;
                
            SoundManager.Instance.GetSfxVolume();
            sfxVolumeSlider.value = sfxVolume;
                
            CameraManager.Instance.GetAimSensitivity();
            aimSensitivitySlider.value = aimSensitivity;
        }
    
        private void VolumeChanged(float value)
        {
            if (SoundManager.Instance) SoundManager.Instance.SetVolume(value);
            string text = GetSliderText(value);
            volumeValueText.text = text;
        }
    
        private void SfxVolumeChanged(float value)
        {
            if (SoundManager.Instance) SoundManager.Instance.SetSfxVolume(value);
            string text = GetSliderText(value);
            sfxVolumeValueText.text = text;
        }
        
        private void AimSensitivityChanged(float value)
        {
            if (CameraManager.Instance) CameraManager.Instance.SetAimSensitivity(value);
            string text = GetSliderText(value);
            aimSensitivityValueText.text = text;
        }

        private void BackToMainMenu()
        {
            gameObject.SetActive(false);
            transform.parent.GetComponent<Selector>().Select();
        }
    
        private string GetSliderText(float value)
        {
            return ((int)(value * 100)).ToString();
        }
    }
}