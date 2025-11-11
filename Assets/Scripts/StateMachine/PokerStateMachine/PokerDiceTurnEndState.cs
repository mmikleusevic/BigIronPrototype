using System;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceTurnEndState : IPokerDiceState
    {
        public static event Action OnTurnEndStarted ;
        
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        public PokerDiceTurnEndState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            diceRoller = pokerDiceGameManager.DiceRoller;
            pokerGame = pokerDiceGameManager.PokerGame;
        }
    
        public void OnEnter()
        {
            OnTurnEndStarted?.Invoke();

            diceRoller.TryAdvanceRollPhase();
            
            diceRoller.ResetDiceHolds(pokerGame.CurrentPlayer);
            
            Debug.Log($"=== End of {pokerGame.CurrentPlayer}'s Turn ===");
            
            if (diceRoller.HaveAllPlayersRolled() && diceRoller.CurrentRollNumber >= diceRoller.MaxRolls)
            {
                pokerDiceGameManager.StateMachine.ChangeState(new PokerDiceEvaluatingState(pokerDiceGameManager));
            }
            else
            {
                pokerGame.NextPlayer();
                pokerDiceGameManager.StateMachine.ChangeState(new PokerDiceTurnStartState(pokerDiceGameManager));
            }
        }
    
        public void OnUpdate() { }
        public void OnExit() { }
    }
}