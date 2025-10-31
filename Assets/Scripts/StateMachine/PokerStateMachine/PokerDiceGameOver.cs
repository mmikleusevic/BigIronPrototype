using System;
using System.Collections.Generic;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceGameOverState : IPokerDiceState
    {
        public static event Action OnGameOverStarted;
        public static event Action<string> OnGameOver;

        private readonly PokerGame pokerGame;
    
        public PokerDiceGameOverState(PokerDiceGameManager manager)
        {
            pokerGame = manager.PokerGame;
        }
    
        public void OnEnter()
        {
            OnGameOverStarted?.Invoke();
                
            Debug.Log("=== GAME OVER ===");
            
            string winner = DetermineWinner();
        
            Debug.Log($"\n*** WINNER: {winner} ***\n");
            
            OnGameOver?.Invoke(winner);
        }
    
        public void OnUpdate() { }
        public void OnExit() { }
    
        private string DetermineWinner()
        {
            string winner = string.Empty;
            int highestScore = -1;
            PokerDiceHandRank highestRank = PokerDiceHandRank.HighCard;
        
            foreach (KeyValuePair<string, PokerDiceHandResult> playerHand in pokerGame.PlayerHands)
            {
                if (playerHand.Value.Rank <= highestRank &&
                    (playerHand.Value.Rank != highestRank ||
                     playerHand.Value.Score <= highestScore)) continue;
                
                highestRank = playerHand.Value.Rank;
                highestScore = playerHand.Value.Score;
                winner = playerHand.Key;
            }
        
            return winner;
        }
    }
}