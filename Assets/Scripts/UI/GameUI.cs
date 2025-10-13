using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private string mainMenuSceneName;
    
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
        backToMainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void OnDisable()
    {
        pauseButton.onClick.RemoveListener(TogglePause);
        resumeButton.onClick.RemoveListener(TogglePause);
        optionsButton.onClick.RemoveListener(OpenOptions);
        backToMainMenuButton.onClick.RemoveListener(BackToMainMenu);
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

    private void BackToMainMenu()
    {
        LevelManager.Instance.LoadSceneAsync(mainMenuSceneName);
    }
}