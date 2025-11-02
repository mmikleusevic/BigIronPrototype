using System;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceTurnStartState : IPokerDiceState
    {
        public static event Action<string> OnTurnStart;
        
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        private float delayTimer;
        private readonly float delayDuration = 1f;
    
        public PokerDiceTurnStartState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            diceRoller = pokerDiceGameManager.DiceRoller;
            pokerGame = pokerDiceGameManager.PokerGame;
        }
    
        public void OnEnter()
        {
            Debug.Log($"=== {pokerGame.CurrentPlayer}'s Turn ===");
            
            diceRoller.ResetDiceHolds(pokerGame.CurrentPlayer);
            
            OnTurnStart?.Invoke(pokerGame.CurrentPlayer);
        
            delayTimer = 0f;
        }

        public void OnUpdate()
        {
            delayTimer += Time.deltaTime;

            if (delayTimer < delayDuration) return;
            
            pokerDiceGameManager.StateMachine.ChangeState(new PokerDiceRollingState(pokerDiceGameManager));
        }
    
        public void OnExit() { }
    }
}