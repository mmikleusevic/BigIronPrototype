using System;
using Cysharp.Threading.Tasks;
using StateMachine;
using StateMachine.PokerStateMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PokerDiceRoom
{
    public class PokerDiceGameController : MonoBehaviour
    {
        [Header("References")] 
        [field: SerializeField] public PokerInputRules PokerInputRules { get; private set; }
        [field: SerializeField] public BaseStateMachine BaseStateMachine { get; private set; }
        [field: SerializeField] public DiceRoller DiceRoller { get; private set; }
        [field: SerializeField] public PokerInputs PokerInputs { get; private set; }
        [field: SerializeField] public PokerGame PokerGame { get; private set; }
        [field: SerializeField] public AssetReference PokerAssetReference { get; private set; }
        [field: SerializeField] public AssetReference GameAssetReference { get; private set; }
        
        public PokerGameEvents PokerGameEvents { get; } = new PokerGameEvents();
        
        public bool IsGameOver { get; private set; }
        
        private void InitializeGame()
        {
            BaseStateMachine.ChangeState(new PokerDiceSetupState(this)).Forget();
        }

        private void OnEnable()
        {
            if (PokerDiceRoomManager.Instance) PokerDiceRoomManager.Instance.OnPokerDiceRoomLoaded += InitializeGame;
            PokerGameEvents.OnGameOverStarted += SetGameOver;
        }

        private void OnDisable()
        {
            if (PokerDiceRoomManager.Instance) PokerDiceRoomManager.Instance.OnPokerDiceRoomLoaded -= InitializeGame;
            PokerGameEvents.OnGameOverStarted -= SetGameOver;
        }

        private void SetGameOver()
        {
            IsGameOver = true;
        }
    }
}