using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extensions;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace UI
{
    public class PauseUI : MonoBehaviour
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
        [SerializeField] private AssetReference mainMenuSceneReference;
        
        private void Awake()
        {
            pausePanel.SetActive(false);
        }

        private void OnEnable()
        {
            pauseButton.onClick.AddListener(TogglePause);
            resumeButton.onClick.AddListener(TogglePause);
            optionsButton.onClick.AddListener(OpenOptions);
            backToMainMenuButton.AddClickAsync(BackToMainMenu);
        }

        private void OnDisable()
        {
            pauseButton.onClick.RemoveListener(TogglePause);
            resumeButton.onClick.RemoveListener(TogglePause);
            optionsButton.onClick.RemoveListener(OpenOptions);
            backToMainMenuButton.onClick.RemoveAllListeners();
        }

        private void TogglePause()
        {
            bool isPaused = GameManager.Instance.TogglePause();
            pausePanel.SetActive(isPaused);
        }

        private void OpenOptions()
        {
            options.gameObject.SetActive(true);
        }

        private async UniTask BackToMainMenu()
        {
            await GameManager.Instance.DisposeGame();
            
            _ = LevelManager.Instance.UnloadAllButPersistentScenesAsync();
            await LevelManager.Instance.LoadSceneAsync(mainMenuSceneReference);

            GameManager.Instance.TogglePause();
        }
    }
}