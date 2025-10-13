using System;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    [SerializeField] private string loaderSceneAddress = "LoaderScene";
    [SerializeField] private string mainMenuSceneAddress = "MainMenuScene";

    public AsyncOperationHandle<SceneInstance> SceneToLoad;
    
    private void Start()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        SceneToLoad = Addressables.LoadSceneAsync(mainMenuSceneAddress, LoadSceneMode.Additive);
        Addressables.LoadSceneAsync(loaderSceneAddress, LoadSceneMode.Additive);
    }
}