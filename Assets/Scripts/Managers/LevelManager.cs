using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapRoom;
using TreeEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        
        [SerializeField] private List<MapNodeLoaderSO> nodeLoaders;
        
        [Tooltip("Scenes that should never be unloaded, referenced by AssetReference but stored by name.")]
        [SerializeField] private List<string> persistentScenes;
        
        private AsyncOperationHandle<SceneInstance>? currentScene;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            currentScene = FindFirstObjectByType<Boot>().SceneToLoad;
            SceneManager.UnloadSceneAsync(0);
        }

        public async Task LoadSceneAsync(AssetReference sceneReference)
        {
            AsyncOperationHandle<SceneInstance> sceneToLoad = Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);
            
            bool isPersistent = false;

            if (currentScene.HasValue) isPersistent = persistentScenes.Contains(currentScene.Value.Result.Scene.name);
            if (!isPersistent && currentScene.HasValue) Addressables.UnloadSceneAsync(currentScene.Value);

            currentScene = sceneToLoad;

            SceneInstance sceneInstance = await sceneToLoad.Task;
            Scene scene = sceneInstance.Scene;
                
            SceneManager.SetActiveScene(scene);
            Debug.Log($"Scene '{scene.name}' is now active.");
        }

        
        public async Task LoadNode(LevelNode node)
        {
            MapNodeLoaderSO mapNodeLoaderSO = nodeLoaders.FirstOrDefault(a => a.LevelNodeType == node.LevelNodeType);
            
            if (mapNodeLoaderSO)
            {
                await mapNodeLoaderSO.LoadAsync(node, this);
            }
            else
            {
                Debug.LogError($"No loader registered for node type: {node.LevelNodeType}");
            }
        }
    }
}