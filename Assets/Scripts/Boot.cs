using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    [SerializeField] private AssetReference loaderScene;
    [SerializeField] private AssetReference mainMenuScene;
    
    private void Start()
    { 
        _ = LoadScene();
    }

    private async UniTask LoadScene()
    {
        await LevelManager.Instance.LoadSceneAsync(loaderScene, false);
        await LevelManager.Instance.LoadSceneAsync(mainMenuScene);
        _ = SceneManager.UnloadSceneAsync(0);
    }
}