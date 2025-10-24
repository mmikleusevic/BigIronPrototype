using System;
using Extensions;
using Player;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public PlayerContext PlayerContext { get; private set; }

        [SerializeField] private PlayerContext playerPrefab;
        [SerializeField] private SceneReferenceSO gameSceneReferenceSO;
        
        private bool isPaused;
        
        private void Awake()
        {
            Instance = this;
        }

        public void InitializeGame()
        {
            PlayerContext = playerPrefab.GetPooledObject<PlayerContext>();
        }
        
        public void DisposeGame()
        {
            PlayerContext.ReturnToPool(playerPrefab);
            PlayerContext = null;
        }

        public bool TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            
            return isPaused;
        }
    }
}