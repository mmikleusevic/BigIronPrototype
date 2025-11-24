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

        private void Start()
        {
            float volume = SoundManager.Instance.GetVolume();
            volumeSlider.value = volume;
        
            float sfxVolume = SoundManager.Instance.GetSfxVolume();
            sfxVolumeSlider.value = sfxVolume;
        }
    
        private void VolumeChanged(float value)
        {
            SoundManager.Instance?.SetVolume(value);
            string text = GetVolumeText(value);
            volumeValueText.text = text;
        }
    
        private void SfxVolumeChanged(float value)
        {
            SoundManager.Instance?.SetSfxVolume(value);
            string text = GetVolumeText(value);
            sfxVolumeValueText.text = text;
        }

        private void BackToMainMenu()
        {
            gameObject.SetActive(false);
            transform.parent.GetComponent<Selector>().SelectFirst();
        }
    
        private string GetVolumeText(float value)
        {
            return ((int)(value * 100)).ToString();
        }
    }
}