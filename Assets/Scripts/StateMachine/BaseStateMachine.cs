using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace StateMachine
{
    public class BaseStateMachine : MonoBehaviour
    {
        private IState CurrentState { get; set; }
        private CancellationTokenSource stateCts;
        
        public async UniTask ChangeState(IState newState)
        {
            stateCts?.Cancel();
            stateCts?.Dispose();
            
            if (CurrentState != null) await CurrentState.OnExit();
            
            stateCts = new CancellationTokenSource();
            
            CancellationToken linkedToken = CancellationTokenSource.CreateLinkedTokenSource(
                stateCts.Token,
                this.GetCancellationTokenOnDestroy()
            ).Token;

            CurrentState = newState;
            
            try
            {
                await CurrentState.OnEnter(linkedToken);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"State {newState.GetType().Name} was cancelled");
            }
        }

        private void Update()
        {
            CurrentState?.OnUpdate();
        }
        
        private void OnDestroy()
        {
            stateCts?.Cancel();
            stateCts?.Dispose();
            
            CurrentState?.OnExit().Forget();
        }
    }
}