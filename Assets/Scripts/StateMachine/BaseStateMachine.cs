using Cysharp.Threading.Tasks;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace StateMachine
{
    public class BaseStateMachine : MonoBehaviour
    {
        private IState CurrentState { get; set; }
        
        public async UniTask ChangeState(IState newState)
        {
            if (CurrentState != null) await CurrentState.OnExit();
            CurrentState = newState;
            if (CurrentState != null) await CurrentState.OnEnter();
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