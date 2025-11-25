using System;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceTurnEndState : IPokerDiceState
    {
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly PokerGameEvents pokerGameEvents;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        public PokerDiceTurnEndState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            pokerGameEvents = pokerDiceGameManager.PokerGameEvents;
            diceRoller = pokerDiceGameManager.DiceRoller;
            pokerGame = pokerDiceGameManager.PokerGame;
        }
    
        public void OnEnter()
        {
            pokerGameEvents.OnTurnEndStarted?.Invoke();

            diceRoller.TryAdvanceRollPhase();
            
            diceRoller.ResetDiceHolds(pokerGame.CurrentPlayer);
            
            Debug.Log($"=== End of {pokerGame.CurrentPlayer}'s Turn ===");
            
            if (diceRoller.HaveAllPlayersRolled() && diceRoller.CurrentRollNumber >= diceRoller.MaxRolls)
            {
                pokerDiceGameManager.BaseStateMachine.ChangeState(new PokerDiceEvaluatingState(pokerDiceGameManager));
            }
            else
            {
                pokerGame.NextPlayer();
                pokerDiceGameManager.BaseStateMachine.ChangeState(new PokerDiceTurnStartState(pokerDiceGameManager));
            }
        }
    
        public void OnUpdate() { }
        public void OnExit() { }
    }
}