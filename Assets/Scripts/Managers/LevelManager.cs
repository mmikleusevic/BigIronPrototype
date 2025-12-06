using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        
        [SerializeField] private List<SceneReferenceSO> persistentScenes;
        [SerializeField] private List<MapNodeLoaderSO> nodeLoaders;
        [SerializeField] private AssetReference mainMenuSceneReference;
        
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> loadedScenes = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public async UniTask LoadSceneAsync(AssetReference sceneReferenceSO,  bool setActive = true)
        {
            SceneInstance sceneInstance;
            AsyncOperationHandle<SceneInstance> handle;
            
            if (IsSceneLoaded(sceneReferenceSO.AssetGUID))
            {
                if (!setActive) return;
                handle = loadedScenes[sceneReferenceSO.AssetGUID];
                sceneInstance = await handle.Task;
            }
            else
            {
                handle = Addressables.LoadSceneAsync(sceneReferenceSO, LoadSceneMode.Additive);
                sceneInstance = await handle.Task;
                loadedScenes[sceneReferenceSO.AssetGUID] = handle;
            }
            
            if (setActive) SceneManager.SetActiveScene(sceneInstance.Scene);

            GameObject playerGameObject = GameManager.Instance?.PlayerCombatant?.gameObject;
            if (playerGameObject) SceneManager.MoveGameObjectToScene(playerGameObject, gameObject.scene);
        }
        
        public async UniTask UnloadSceneAsync(string sceneGUID)
        {
            if (!loadedScenes.TryGetValue(sceneGUID, out AsyncOperationHandle<SceneInstance> handle)) return;
            
            AsyncOperationHandle<SceneInstance> sceneToUnload = Addressables.UnloadSceneAsync(handle);
            await sceneToUnload.Task;
            
            loadedScenes.Remove(sceneGUID);
        }

        public async UniTask UnloadAllButPersistentScenesAsync()
        {
            List<UniTask> unloadTasks = new List<UniTask>();
    
            foreach (KeyValuePair<string, AsyncOperationHandle<SceneInstance>> loadedScenePair in loadedScenes)
            {
                if (!IsPersistentScene(loadedScenePair.Key)) unloadTasks.Add(UnloadSceneAsync(loadedScenePair.Key));
            }
    
            await UniTask.WhenAll(unloadTasks);
        }
        
        private bool IsSceneLoaded(string assetGUID)
        {
            return loadedScenes.ContainsKey(assetGUID);
        }

        private bool IsPersistentScene(string sceneGUID)
        {
            return persistentScenes.Any(s => s.AssetGUID == sceneGUID);
        }
        
        public async UniTask LoadNode(LevelNode node)
        {
            MapNodeLoaderSO mapNodeLoaderSO = nodeLoaders.FirstOrDefault(a => a.LevelNodeType == node.LevelNodeType);
            
            if (mapNodeLoaderSO) await mapNodeLoaderSO.LoadAsync(node, this);
        }

        public async UniTask LoadMainMenu()
        {
            if (UIFocusManager.Instance) UIFocusManager.Instance.ClearFocus();
            if (InputManager.Instance) InputManager.Instance.StartingMapsSetup();
            
            UnloadAllButPersistentScenesAsync().Forget();
            await LoadSceneAsync(mainMenuSceneReference);

            if (GameManager.Instance) GameManager.Instance.TogglePause();
            if (GameManager.Instance) Destroy(GameManager.Instance.PlayerCombatant.gameObject);
        }
    }
}