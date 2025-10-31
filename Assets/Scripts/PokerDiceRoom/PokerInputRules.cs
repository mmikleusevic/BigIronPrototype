using System;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace PokerDiceRoom
{
    public class PokerInputRules : MonoBehaviour
    {
        public event Action OnRulesChanged;
        
        public bool CanRoll { get; private set; }
        public bool CanHold { get; private set; }
        public bool CanContinue { get; private set; }
        public bool CanSelect { get; private set; }

        private void OnEnable()
        {
            PokerDiceRollingState.OnDiceRollingStarted += SetRollingPhase;
            PokerDiceRollingState.OnDiceRollingEnded += Reset;
            PokerDiceEvaluatingState.OnDiceEvaluationStarted += SetEvaluationPhase;
            PokerDiceGameOverState.OnGameOverStarted += SetGameOverPhase;
        }

        private void OnDisable()
        {
            PokerDiceRollingState.OnDiceRollingStarted -= SetRollingPhase;
            PokerDiceRollingState.OnDiceRollingEnded -= Reset;
            PokerDiceEvaluatingState.OnDiceEvaluationStarted -= SetEvaluationPhase;
            PokerDiceGameOverState.OnGameOverStarted -= SetGameOverPhase;
        }

        private void SetRollingPhase(int currentRoll, int maxRolls)
        {
            CanRoll = currentRoll <= maxRolls;
            CanHold = currentRoll > 1 && currentRoll <= maxRolls;
            CanSelect = currentRoll > 1 && currentRoll <= maxRolls;
            CanContinue = false;
            
            OnRulesChanged?.Invoke();
        }

        private void SetEvaluationPhase()
        {
            CanRoll = false;
            CanHold = false;
            CanContinue = false;
            CanSelect = false;
            
            OnRulesChanged?.Invoke();
        }

        private void SetGameOverPhase()
        {
            CanContinue = true;
            
            OnRulesChanged?.Invoke();
        }

        private void Reset()
        {
            CanRoll = false;
            CanHold = false;
            CanContinue = false;
            CanSelect = false;
            
            OnRulesChanged?.Invoke();
        }
    }
}