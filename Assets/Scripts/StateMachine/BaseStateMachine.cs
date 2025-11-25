using StateMachine.PokerStateMachine;
using UnityEngine;

namespace StateMachine
{
    public class BaseStateMachine : MonoBehaviour
    {
        private IPokerDiceState CurrentState { get; set; }
        
        public void ChangeState(IPokerDiceState newPokerDiceState)
        {
            CurrentState?.OnExit();
            CurrentState = newPokerDiceState;
            CurrentState?.OnEnter();
        }

        private void Update()
        {
            CurrentState?.OnUpdate();
        }
        
        private void OnDestroy()
        {
            CurrentState?.OnExit();
        }
    }
}