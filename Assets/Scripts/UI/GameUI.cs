using System;
using System.Threading.Tasks;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
    
        [Space(20)]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private Button resumeButton;
    
        [Header("Options")]
        [SerializeField] private Button optionsButton;
        [SerializeField] private Options options;
    
        [Space(20)]
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private AssetReference mainMenuScene;
    
        private bool isPaused;

        private void Awake()
        {
            pausePanel.SetActive(false);
        }

        private void OnEnable()
        {
            pauseButton.onClick.AddListener(TogglePause);
            resumeButton.onClick.AddListener(TogglePause);
            optionsButton.onClick.AddListener(OpenOptions);
            backToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);
        }

        private void OnDisable()
        {
            pauseButton.onClick.RemoveListener(TogglePause);
            resumeButton.onClick.RemoveListener(TogglePause);
            optionsButton.onClick.RemoveListener(OpenOptions);
            backToMainMenuButton.onClick.RemoveListener(OnBackToMainMenuClicked);
        }

        private void TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            pausePanel.SetActive(isPaused);
        }

        private void OpenOptions()
        {
            options.gameObject.SetActive(true);
        }
        
        private async void OnBackToMainMenuClicked()
        {
            try
            {
                backToMainMenuButton.interactable = false;
            
                await BackToMainMenu();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to go back to main menu: {ex}");
            }
            finally
            {
                backToMainMenuButton.interactable = true;
            }
        }

        private async Task BackToMainMenu()
        {
            await LevelManager.Instance.LoadSceneAsync(mainMenuScene);
        }
    }
}