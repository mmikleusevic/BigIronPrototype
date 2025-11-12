using System;
using System.Collections.Generic;
using System.Linq;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceGameOverState : IPokerDiceState
    {
        public static event Action OnGameOverStarted;
        public static event Action<int> OnGameOver;

        private readonly PokerGame pokerGame;
    
        public PokerDiceGameOverState(PokerDiceGameManager manager)
        {
            pokerGame = manager.PokerGame;
        }
    
        public void OnEnter()
        {
            OnGameOverStarted?.Invoke();
                
            Debug.Log("=== GAME OVER ===");
            
            List<PokerDiceHandResult> winners = DetermineWinners();
            if (winners.Count == 1)
            {
                Debug.Log($"🏆 Winner: {winners[0].PlayerName}");

                if (winners[0].PlayerName == GameStrings.PLAYER) OnGameOver?.Invoke(pokerGame.Wager);
            }
            else
            {
                string tiedNames = string.Join(", ", winners.Select(w => w.PlayerName));
                Debug.Log($"🤝 It's a tie between {tiedNames}!");
            }
        }
    
        public void OnUpdate() { }
        public void OnExit() { }
    
        private List<PokerDiceHandResult> DetermineWinners()
        {
            PokerDiceHandRank topRank = pokerGame.PlayerHands.Max(x => x.Value.Rank);
            List<PokerDiceHandResult> candidates = pokerGame.PlayerHands
                .Where(x => x.Value.Rank == topRank)
                .Select(x => x.Value)
                .ToList();

            int topScore = candidates.Max(x => x.Score);
            List<PokerDiceHandResult> winners = candidates.Where(x => x.Score == topScore).ToList();

            return winners;
        }
    }
}