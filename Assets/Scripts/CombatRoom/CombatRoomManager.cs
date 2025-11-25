using System;
using Cysharp.Threading.Tasks;
using Managers;
using Player;
using StateMachine;
using StateMachine.CombatStateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatRoom
{
    public class CombatRoomManager : MonoBehaviour
    {
        public static CombatRoomManager Instance { get; private set; }

        [field: SerializeField] public BaseStateMachine BaseStateMachine { get; private set; }
        
        public Vector3[] EnemyPositions;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameObject playerGameObject = GameManager.Instance.PlayerContext.gameObject;
            Scene targetScene = gameObject.scene; 
    
            SceneManager.MoveGameObjectToScene(playerGameObject, targetScene);
            
            BaseStateMachine.ChangeState(new CombatRoomSetupState(this));
        }
    }
}