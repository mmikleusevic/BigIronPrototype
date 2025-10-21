using System;
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

    public AsyncOperationHandle<SceneInstance> SceneToLoad;
    
    private void Start()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        SceneToLoad = Addressables.LoadSceneAsync(mainMenuScene, LoadSceneMode.Additive);
        Addressables.LoadSceneAsync(loaderScene, LoadSceneMode.Additive);
    }
}