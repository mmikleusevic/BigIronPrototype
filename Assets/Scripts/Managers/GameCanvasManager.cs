using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameCanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private SceneReferenceSO gameSceneReferenceSO;
        
        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
        
        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
        
        private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
        {
            if (newScene.name == gameSceneReferenceSO.AssetName)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            canvas.gameObject.SetActive(true);
        }
        
        private void Hide()
        {
            canvas.gameObject.SetActive(false);
        }
    }
}