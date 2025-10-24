using System;
using Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private AssetReference gameSceneReference;
        
        private void OnEnable()
        {
            if (!LevelManager.Instance) return;
            
            LevelManager.Instance.OnSceneLoaded += SceneLoaded;
        }

        private void OnDisable()
        {
            if (!LevelManager.Instance) return;
            
            LevelManager.Instance.OnSceneLoaded -= SceneLoaded;
        }

        private void SceneLoaded(string sceneGUID, bool visible)
        {
            bool isThisScene = sceneGUID == gameSceneReference.AssetGUID;
            
            if (!visible && isThisScene) return;
            gameObject.SetActive(isThisScene);
        }
    }
}
