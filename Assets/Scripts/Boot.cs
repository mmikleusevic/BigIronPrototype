using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    [SerializeField] private AssetReference loaderScene;
    [SerializeField] private AssetReference mainMenuScene;
    
    private void Start()
    {
        LoadScene().Forget();
    }

    private async UniTask LoadScene()
    {
        if (LevelManager.Instance)
        {
            await LevelManager.Instance.LoadSceneAsync(loaderScene, false);
            await LevelManager.Instance.LoadSceneAsync(mainMenuScene);
        }
    }
}