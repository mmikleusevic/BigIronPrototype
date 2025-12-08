using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceTurnStartState : IState
    {
        private readonly PokerGameEvents pokerGameEvents;
        private readonly PokerDiceGameController pokerDiceGameController;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        private readonly float delayDuration = 1f;
    
        public PokerDiceTurnStartState(PokerDiceGameController controller)
        {
            pokerDiceGameController = controller;
            pokerGameEvents = pokerDiceGameController.PokerGameEvents;
            diceRoller = pokerDiceGameController.DiceRoller;
            pokerGame = pokerDiceGameController.PokerGame;
        }
    
        public async UniTask OnEnter(CancellationToken externalToken)
        {
            Debug.Log($"=== {pokerGame.CurrentPlayer}'s Turn ===");
            
            diceRoller.ResetDiceHolds(pokerGame.CurrentPlayer);
            pokerGameEvents?.OnTurnStart?.Invoke(pokerGame.CurrentPlayer);
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayDuration), cancellationToken: externalToken);
        
            if (pokerGame.CurrentPlayer.IsAI)
            {
                await pokerDiceGameController.BaseStateMachine.ChangeState(new PokerDiceAIRollingState(pokerDiceGameController));    
            }
            else
            {
                await pokerDiceGameController.BaseStateMachine.ChangeState(new PokerDiceRollingState(pokerDiceGameController));
            }
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}