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
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private OptionsUI optionsUI;
        
        [SerializeField] private AssetReference mainMenuAssetReference;
        [SerializeField] private AssetReference gameAssetReference;
    
        public Selector selector;
        
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
    
        private async UniTask PlayGame()
        {
            LevelManager.Instance.UnloadSceneAsync(mainMenuAssetReference.AssetGUID).Forget();
            await LevelManager.Instance.LoadSceneAsync(gameAssetReference);
        }

        private void OpenOptions()
        {
            optionsUI.gameObject.SetActive(true);
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