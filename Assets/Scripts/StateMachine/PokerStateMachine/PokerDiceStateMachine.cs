using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceStateMachine : MonoBehaviour
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