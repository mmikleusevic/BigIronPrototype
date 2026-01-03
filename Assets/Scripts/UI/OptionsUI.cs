using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        
        private GameObject returnSelection;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            volumeSlider.onValueChanged.AddListener(VolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(SfxVolumeChanged);
            aimSensitivitySlider.onValueChanged.AddListener(AimSensitivityChanged);
            backButton.onClick.AddListener(BackToPreviousMenu);
        }
    
        private void OnDisable()
        {
            volumeSlider.onValueChanged.RemoveListener(VolumeChanged);
            sfxVolumeSlider.onValueChanged.RemoveListener(SfxVolumeChanged);
            aimSensitivitySlider.onValueChanged.RemoveListener(AimSensitivityChanged);
            backButton.onClick.RemoveListener(BackToPreviousMenu);
        }

        private void Start()
        {
            if (SoundManager.Instance)
            {
                float volume = SoundManager.Instance.GetVolume();
                volumeSlider.value = volume;
                VolumeChanged(volume);
                
                float sfxVolume = SoundManager.Instance.GetSfxVolume();
                sfxVolumeSlider.value = sfxVolume;
                SfxVolumeChanged(sfxVolume);
            }

            if (!CameraManager.Instance) return;
            
            float aimSensitivity = CameraManager.Instance.GetAimSensitivity();
            aimSensitivitySlider.value = aimSensitivity;
            AimSensitivityChanged(aimSensitivity);
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
        
        private void BackToPreviousMenu()
        {
            gameObject.SetActive(false);

            if (returnSelection) EventSystem.current.SetSelectedGameObject(returnSelection);
        }
    
        private string GetSliderText(float value)
        {
            return Mathf.RoundToInt(value * 100).ToString();
        }

        public void Show(GameObject returnSelection)
        {
            this.returnSelection = returnSelection;
            gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);
        }
    }
}