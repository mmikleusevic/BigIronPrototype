using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions;
using Player;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public PlayerContext PlayerContext { get; private set; }

        [SerializeField] private PlayerContext playerPrefab;
        [SerializeField] private SceneReferenceSO gameSceneReferenceSO;
        
        public List<IClearable> Clearables { get; set; } = new List<IClearable>();
        
        private bool isPaused;
        
        private void Awake()
        {
            Instance = this;
        }

        public void InitializeGame()
        {
            PlayerContext = playerPrefab.GetPooledObject<PlayerContext>();
        }
        
        public async UniTask DisposeGame()
        {
            PlayerContext.ReturnToPool();
            PlayerContext = null;

            foreach (IClearable clearable in Clearables)
            {
                clearable.ReturnToPool();
            }
            
            Clearables.Clear();
            
            await UniTask.CompletedTask;
        }

        public bool TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            
            return isPaused;
        }
    }
}