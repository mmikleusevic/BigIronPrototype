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
        public event Action OnPlayerInitialized;
        public event Action OnRoomPassed;
        
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
            PlayerContext = Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation);
            
            OnPlayerInitialized?.Invoke();
        }

        public bool TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            
            return isPaused;
        }

        public void RoomPassed()
        {
            OnRoomPassed?.Invoke();
        }
    }
}