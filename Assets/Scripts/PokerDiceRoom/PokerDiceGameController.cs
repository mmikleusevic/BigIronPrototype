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
        
        public PokerGameEvents PokerGameEvents { get; private set; } = new PokerGameEvents();
        
        public bool IsGameOver { get; private set; }

        private void Start()
        {
            InitializeGame().Forget();
        }

        private async UniTask InitializeGame()
        {
            await BaseStateMachine.ChangeState(new PokerDiceSetupState(this));
        }

        private void OnEnable()
        {
            PokerGameEvents.OnGameOverStarted += SetGameOver;
        }

        private void OnDisable()
        {
            PokerGameEvents.OnGameOverStarted -= SetGameOver;
        }

        private void SetGameOver()
        {
            IsGameOver = true;
        }
    }
}