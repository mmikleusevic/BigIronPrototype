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
        [SerializeField] private Button backToMainMenuButton;
        
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
                if (InputManager.Instance) InputManager.Instance.EnableOnlyUIMap();
                if (!UIFocusManager.Instance) return;
                
                UIFocusManager.Instance
                    .SaveFocus(EventSystem.current.currentSelectedGameObject
                        ?.GetComponent<Selectable>(), Cursor.lockState);
                selector.Select();
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                if (InputManager.Instance) InputManager.Instance.RestoreMaps();
                
                if (!UIFocusManager.Instance) return;
                
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
            if (LevelManager.Instance) await LevelManager.Instance.LoadMainMenu();
        }
    }
}