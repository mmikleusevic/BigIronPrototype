using System;
using Extensions;
using Player;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public PlayerContext PlayerContext { get; private set; }

        [SerializeField] private PlayerContext playerPrefab;
        
        private void Awake()
        {
            Instance = this;
        }

        public void InitializeGame()
        {
            PlayerContext = playerPrefab.GetPooledObject<PlayerContext>();
            DontDestroyOnLoad(PlayerContext.gameObject);
        }
    }
}