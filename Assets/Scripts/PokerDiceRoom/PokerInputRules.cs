using System;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace PokerDiceRoom
{
    public class PokerInputRules : MonoBehaviour
    {
        [SerializeField] private PokerDiceGameController pokerDiceGameController;
        
        public event Action OnRulesChanged;
        
        public bool CanRoll { get; private set; }
        public bool CanHold { get; private set; }
        public bool CanEnd { get; private set; }
        public bool CanSelect { get; private set; }
        public bool CanMove { get; private set; }
        
        private PokerGameEvents PokerGameEvents => pokerDiceGameController.PokerGameEvents;

        private void OnEnable()
        {
            PokerGameEvents.OnDiceRollingStarted += SetRollingPhase;
            PokerGameEvents.OnDiceRollingEnded += Reset;
            PokerGameEvents.OnDiceEvaluationStarted += SetEvaluationPhase;
            PokerGameEvents.OnGameOverStarted += SetGameOverPhase;
        }

        private void OnDisable()
        {
            PokerGameEvents.OnDiceRollingStarted -= SetRollingPhase;
            PokerGameEvents.OnDiceRollingEnded -= Reset;
            PokerGameEvents.OnDiceEvaluationStarted -= SetEvaluationPhase;
            PokerGameEvents.OnGameOverStarted -= SetGameOverPhase;
        }

        private void SetRollingPhase(int currentRoll, int maxRolls)
        {
            CanMove = currentRoll > 1;
            CanRoll = currentRoll <= maxRolls;
            CanHold = currentRoll > 1 && currentRoll <= maxRolls;
            CanSelect = currentRoll > 1 && currentRoll <= maxRolls;
            CanEnd = false;
            
            OnRulesChanged?.Invoke();
        }

        private void SetEvaluationPhase()
        {
            CanMove = false;
            CanRoll = false;
            CanHold = false;
            CanEnd = false;
            CanSelect = false;
            
            OnRulesChanged?.Invoke();
        }

        private void SetGameOverPhase()
        {
            CanEnd = true;
            
            OnRulesChanged?.Invoke();
        }

        private void Reset()
        {
            CanMove = false;
            CanRoll = false;
            CanHold = false;
            CanEnd = false;
            CanSelect = false;
            
            OnRulesChanged?.Invoke();
        }
    }
}