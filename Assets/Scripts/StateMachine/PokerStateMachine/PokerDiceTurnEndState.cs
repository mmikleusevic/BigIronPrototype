using System;
using Cysharp.Threading.Tasks;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceTurnEndState : IState
    {
        private readonly PokerDiceGameController pokerDiceGameController;
        private readonly PokerGameEvents pokerGameEvents;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        public PokerDiceTurnEndState(PokerDiceGameController controller)
        {
            pokerDiceGameController = controller;
            pokerGameEvents = pokerDiceGameController.PokerGameEvents;
            diceRoller = pokerDiceGameController.DiceRoller;
            pokerGame = pokerDiceGameController.PokerGame;
        }
    
        public async UniTask OnEnter()
        {
            pokerGameEvents.OnTurnEndStarted?.Invoke();

            diceRoller.TryAdvanceRollPhase();
            
            diceRoller.ResetDiceHolds(pokerGame.CurrentPlayer);
            
            Debug.Log($"=== End of {pokerGame.CurrentPlayer}'s Turn ===");
            
            if (diceRoller.HaveAllPlayersRolled() && diceRoller.CurrentRollNumber >= diceRoller.MaxRolls)
            {
                await pokerDiceGameController.BaseStateMachine.ChangeState(new PokerDiceEvaluatingState(pokerDiceGameController));
            }
            else
            {
                pokerGame.NextPlayer();
                await pokerDiceGameController.BaseStateMachine.ChangeState(new PokerDiceTurnStartState(pokerDiceGameController));
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