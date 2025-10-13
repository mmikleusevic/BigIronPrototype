using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SceneManager = Managers.SceneManager;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject optionsPanel;
    
    [SerializeField] private string gameSceneName;
    
    private void OnEnable()
    {
        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(OpenOptions);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(PlayGame);
        optionsButton.onClick.RemoveListener(OpenOptions);
        exitButton.onClick.RemoveListener(ExitGame);
    }

    private void PlayGame()
    {
        SceneManager.Instance.LoadSceneAsync(gameSceneName);
    }

    private void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}