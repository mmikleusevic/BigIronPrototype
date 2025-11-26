using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extensions;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
        [SerializeField] private OptionsUI optionsUI;

        [Space(20)] 
        [SerializeField] private string uiActionMapName;
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private AssetReference mainMenuSceneReference;
        
        public Selector selector;
        
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

        public void TogglePause()
        {
            bool isPaused = GameManager.Instance.TogglePause();
            pausePanel.SetActive(isPaused);

            if (isPaused)
            {
                InputManager.Instance.EnableOnlyUIMap();
                UIFocusManager.Instance.SaveFocus(EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>());
                selector.Select();
            }
            else
            {
                InputManager.Instance.RestoreMaps();
                UIFocusManager.Instance.RestoreBeforePause();
                UIFocusManager.Instance.RestoreFocus();
            }
        }

        private void OpenOptions()
        {
            optionsUI.gameObject.SetActive(true);
        }

        private async UniTask BackToMainMenu()
        {
            UIFocusManager.Instance.ClearFocus();
            InputManager.Instance.StartingMapsSetup();
            
            LevelManager.Instance.UnloadAllButPersistentScenesAsync().Forget();
            await LevelManager.Instance.LoadSceneAsync(mainMenuSceneReference);

            GameManager.Instance.TogglePause();
        }
    }
}