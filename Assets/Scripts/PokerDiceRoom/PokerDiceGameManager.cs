using System;
using System.Collections.Generic;
using Extensions;
using Managers;
using StateMachine;
using StateMachine.PokerStateMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace PokerDiceRoom
{
    public class PokerDiceGameManager : MonoBehaviour
    {
        public static PokerDiceGameManager Instance { get; private set; }
        
        [Header("References")] 
        [field: SerializeField] public PokerInputRules PokerInputRules { get; private set; }
        [field: SerializeField] public StateMachine.StateMachine StateMachine { get; private set; }
        [field: SerializeField] public DiceRoller DiceRoller { get; private set; }
        [field: SerializeField] public PokerInputs PokerInputs { get; private set; }
        [field: SerializeField] public PokerGame PokerGame { get; private set; }
        [field: SerializeField] public AssetReference PokerAssetReference { get; private set; }
        [field: SerializeField] public AssetReference GameAssetReference { get; private set; }
        
        public PokerGameEvents PokerGameEvents { get; private set; } = new PokerGameEvents();
        
        public bool IsGameOver { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void InitializeGame()
        {
            StateMachine.ChangeState(new PokerDiceSetupState(this));
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