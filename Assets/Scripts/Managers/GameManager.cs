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
        public event Action OnGameStarted;
        public event Action OnRoomPassed;
        public event Action<bool, bool> OnGameOver;
        
        public static GameManager Instance { get; private set; }

        public PlayerCombatant PlayerCombatant { get; private set; }

        [SerializeField] private PlayerCombatant playerPrefab;
        [SerializeField] private SceneReferenceSO gameSceneReferenceSO;
        
        private bool isPaused;
        private bool isGameOver;
        
        public bool IsPaused => isPaused;
        
        private void Awake()
        {
            Instance = this;
        }

        public void InitializeGame()
        {
            PlayerCombatant = Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation);
            isGameOver = false;
            
            OnGameStarted?.Invoke();
        }

        public bool TogglePause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            
            return isPaused;
        }
        
        public void Pause()
        {
            isPaused = true;
            Time.timeScale = 0;
        }
        
        public void Unpause()
        {
            isPaused = false;
            Time.timeScale = 1;
        }

        public void RoomPassed()
        {
            OnRoomPassed?.Invoke();
        }

        public void GameOver(bool hasWon)
        {
            isGameOver = true;
            OnGameOver?.Invoke(hasWon, isGameOver);
        }
    }
}