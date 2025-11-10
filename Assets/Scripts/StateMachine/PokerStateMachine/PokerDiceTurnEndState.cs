using System;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceTurnEndState : IPokerDiceState
    {
        public static event Action OnTurnEndStarted ;
        
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly PokerGame pokerGame;
        
        public PokerDiceTurnEndState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            pokerGame = pokerDiceGameManager.PokerGame;
        }
    
        public void OnEnter()
        {
            OnTurnEndStarted?.Invoke();
            
            Debug.Log($"=== End of {pokerGame.CurrentPlayer}'s Turn ===");
            
            pokerGame.NextPlayer();
            
            pokerDiceGameManager.StateMachine.ChangeState(new PokerDiceTurnStartState(pokerDiceGameManager));
        }
    
        public void OnUpdate() { }
        public void OnExit() { }
    }
}