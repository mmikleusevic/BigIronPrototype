using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MapRoom;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
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
        [SerializeField] private AssetReference gameAssetReference;
        
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> loadedScenes = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();

        private void Awake()
        {
            Instance = this;
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

        private async UniTask UnloadAllButPersistentScenesAsync()
        {
            List<string> scenesToUnload = loadedScenes.Keys.Where(guid => !IsPersistentScene(guid)).ToList();
    
            List<UniTask> unloadTasks = new List<UniTask>();

            foreach (string sceneGUID in scenesToUnload)
            {
                unloadTasks.Add(UnloadSceneAsync(sceneGUID));
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
            
            await UnloadAllButPersistentScenesAsync();
            await LoadSceneAsync(mainMenuSceneReference);

            if (GameManager.Instance) GameManager.Instance.TogglePause();
            if (TargetModifierManager.Instance) TargetModifierManager.Instance.RemoveAllModifiers();
            if (GameManager.Instance) Destroy(GameManager.Instance.PlayerCombatant.gameObject);
        }

        public async UniTask UnloadSceneActivateGame(AssetReference sceneReference)
        {
            if (EventSystem.current) EventSystem.current.SetSelectedGameObject(null);

            await UnloadSceneAsync(sceneReference.AssetGUID);
            await LoadSceneAsync(gameAssetReference);

            if (InputManager.Instance) InputManager.Instance.EnableOnlyUIMap();
            if (GameManager.Instance) GameManager.Instance.RoomPassed();
        }
    }
}