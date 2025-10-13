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

    private void Start()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        Addressables.LoadSceneAsync(loaderSceneAddress, LoadSceneMode.Additive);
        Addressables.LoadSceneAsync(mainMenuSceneAddress);
    }

    public void UnloadScene(AsyncOperationHandle<SceneInstance> sceneToUnload)
    {
        Addressables.UnloadSceneAsync(sceneToUnload);
    }
}