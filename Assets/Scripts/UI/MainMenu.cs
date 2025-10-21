using System;
using System.Threading.Tasks;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Options options;
    
    [SerializeField] private AssetReference gameScene;
    
    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayGameClicked);
        optionsButton.onClick.AddListener(OpenOptions);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayGameClicked);
        optionsButton.onClick.RemoveListener(OpenOptions);
        exitButton.onClick.RemoveListener(ExitGame);
    }

    private async void OnPlayGameClicked()
    {
        try
        {
            playButton.interactable = false;
            
            await PlayGame();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to start playing game: {ex}");
        }
        finally
        {
            playButton.interactable = true;
        }
    }
    
    private async Task PlayGame()
    {
        await LevelManager.Instance.LoadSceneAsync(gameScene);
    }

    private void OpenOptions()
    {
        options.gameObject.SetActive(true);
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