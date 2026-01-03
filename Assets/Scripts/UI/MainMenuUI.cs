using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extensions;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private OptionsUI optionsUI;
        
        [SerializeField] private AssetReference mainMenuAssetReference;
        [SerializeField] private AssetReference gameAssetReference;
        

        private void OnEnable()
        {
            playButton.AddClickAsync(PlayGame);
            optionsButton.onClick.AddListener(OpenOptions);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveAllListeners();
            optionsButton.onClick.RemoveListener(OpenOptions);
            exitButton.onClick.RemoveListener(ExitGame);
        }

        private void Start()
        {
            SelectFirst();
        }

        private async UniTask PlayGame()
        {
            if (LevelManager.Instance)
            {
                await LevelManager.Instance.UnloadSceneAsync(mainMenuAssetReference.AssetGUID);
                await LevelManager.Instance.LoadSceneAsync(gameAssetReference);
            }
        }

        private void OpenOptions()
        {
            optionsUI.Show(optionsButton.gameObject);
        }

        public void SelectFirst()
        {
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
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
}