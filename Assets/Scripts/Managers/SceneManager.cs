using System;
using System.Collections;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }
        
        private AsyncOperationHandle<SceneInstance> currentScene;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            currentScene = FindFirstObjectByType<Boot>().SceneToLoad;
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0);
        }

        public void LoadSceneAsync(string sceneName)
        {
            AsyncOperationHandle<SceneInstance> sceneToLoad = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Addressables.UnloadSceneAsync(currentScene);
            currentScene = sceneToLoad;

            currentScene.Completed += OnSceneLoaded;
        }
        
        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                SceneInstance loadedScene = handle.Result;
                Scene scene = loadedScene.Scene;
                
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
                Debug.Log($"Scene '{scene.name}' is now active.");
            }
            else
            {
                Debug.LogError($"Failed to load scene '{handle.Result.Scene.name}'.");
            }
            
            currentScene.Completed -= OnSceneLoaded;
        }
        
        public void UnloadScene()
        {
            Addressables.UnloadSceneAsync(currentScene);
        }
    }
}