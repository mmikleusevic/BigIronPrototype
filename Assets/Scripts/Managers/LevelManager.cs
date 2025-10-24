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
        public event Action<string, bool> OnSceneLoaded;
        
        [SerializeField] private List<SceneReferenceSO> persistentScenes;
        [SerializeField] private List<MapNodeLoaderSO> nodeLoaders;
        
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> loadedScenes = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public async Task LoadSceneAsync(AssetReference sceneReferenceSO,  bool setActive = true)
        {
            if (IsSceneLoaded(sceneReferenceSO.AssetGUID)) return;

            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneReferenceSO, LoadSceneMode.Additive);
            SceneInstance sceneInstance = await handle.Task;

            loadedScenes[sceneReferenceSO.AssetGUID] = handle;

            if (setActive) SceneManager.SetActiveScene(sceneInstance.Scene);
            
            OnSceneLoaded?.Invoke(sceneReferenceSO.AssetGUID, true);
        }
        
        public async Task UnloadSceneAsync(string sceneGUID)
        {
            if (!loadedScenes.TryGetValue(sceneGUID, out AsyncOperationHandle<SceneInstance> handle)) return;
            
            AsyncOperationHandle<SceneInstance> sceneToUnload = Addressables.UnloadSceneAsync(handle);
            await sceneToUnload.Task;
            
            loadedScenes.Remove(sceneGUID);
        }

        public async Task UnloadAllButPersistentScenesAsync()
        {
            List<Task> unloadTasks = new List<Task>();
    
            foreach (KeyValuePair<string, AsyncOperationHandle<SceneInstance>> loadedScenePair in loadedScenes)
            {
                if (!IsPersistentScene(loadedScenePair.Key)) unloadTasks.Add(UnloadSceneAsync(loadedScenePair.Key));
            }
    
            await Task.WhenAll(unloadTasks);
        }
        
        private bool IsSceneLoaded(string assetGUID)
        {
            return loadedScenes.ContainsKey(assetGUID);
        }

        private bool IsPersistentScene(string sceneGUID)
        {
            return persistentScenes.Any(s => s.AssetGUID == sceneGUID);
        }
        
        public async Task LoadNode(LevelNode node)
        {
            MapNodeLoaderSO mapNodeLoaderSO = nodeLoaders.FirstOrDefault(a => a.LevelNodeType == node.LevelNodeType);
            
            if (mapNodeLoaderSO) await mapNodeLoaderSO.LoadAsync(node, this);
        }
    }
}