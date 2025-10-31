using System;
using System.Collections.Generic;
using Extensions;
using Managers;
using StateMachine;
using StateMachine.PokerStateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PokerDiceRoom
{
    public class PokerDiceGameManager : MonoBehaviour
    {
        [Header("References")] 
        public PokerInputRules InputRules { get; } = new PokerInputRules();
        [field: SerializeField] public PokerDiceStateMachine StateMachine { get; private set; }
        [field: SerializeField] public DiceRoller DiceRoller { get; private set; }
        [field: SerializeField] public PokerInputHandler PokerInputHandler { get; private set; }
        [field: SerializeField] public PokerGame PokerGame { get; private set; }
        
        private void Start()
        {
            StateMachine.ChangeState(new PokerDiceSetupState(this));
        }
    }
}