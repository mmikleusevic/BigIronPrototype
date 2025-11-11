using System;
using System.Collections.Generic;
using System.Linq;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceEvaluatingState : IPokerDiceState
    {
        public static event Action OnDiceEvaluationStarted;
        public static event Action<PokerDiceHandResult> OnHandEvaluated;
        
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly PokerGame pokerGame;
    
        private readonly float displayDuration = 3f;
        private float displayTimer;
    
        public PokerDiceEvaluatingState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            pokerGame = pokerDiceGameManager.PokerGame;
        }
    
        public void OnEnter()
        {
            //TODO test more 
            
            OnDiceEvaluationStarted?.Invoke();
            Debug.Log("=== Evaluating Hands ===");

            Dictionary<PokerPlayer, PokerDiceHandResult> results = new();

            foreach (var (playerName, rolls) in pokerGame.PlayerRolls)
            {
                PokerDiceHandResult result = PokerDiceHandEvaluation.EvaluateHand(playerName, rolls);
                pokerGame.SetPlayerHand(playerName, result);
                results[playerName] = result;
                OnHandEvaluated?.Invoke(result);
            }

            List<PokerDiceHandResult> winners = DetermineWinners(results);
            DisplayEvaluationReport(results, winners);

            displayTimer = 0f;
        }
    
        public void OnUpdate()
        {
            displayTimer += Time.deltaTime;

            if (displayTimer < displayDuration) return;
            
            if (pokerGame.AllPlayersFinished())
            {
                pokerDiceGameManager.StateMachine.ChangeState(new PokerDiceGameOverState(pokerDiceGameManager));
            }
        }
    
        public void OnExit() { }
        
        
        // TODO remove methods later
        private void DisplayEvaluationReport(Dictionary<PokerPlayer, PokerDiceHandResult> results, List<PokerDiceHandResult> winners)
        {
            Debug.Log("=== Hand Evaluation Results ===");

            foreach (var (player, result) in results)
            {
                Debug.Log($"{player}: {result.Description} (Score: {result.Score})");
            }

            if (winners.Count == 1)
            {
                // One winner
                var winner = winners[0];
                Debug.Log($"🏆 Winner: {winner.PlayerName} with {winner.Description}!");
            }
            else
            {
                // Tie
                string tiedPlayers = string.Join(", ", winners.Select(w => w.PlayerName));
                Debug.Log($"🤝 It's a tie between {tiedPlayers} with {winners[0].Description}!");
            }
        }
        
        private List<PokerDiceHandResult> DetermineWinners(Dictionary<PokerPlayer, PokerDiceHandResult> results)
        {
            // Sort by rank and score
            List<PokerDiceHandResult> ordered = results.Values
                .OrderByDescending(r => r.Rank)
                .ThenByDescending(r => r.Score)
                .ToList();
            
            PokerDiceHandResult top = ordered.First();
            
            List<PokerDiceHandResult> tiedWinners = ordered
                .Where(r => r.Rank == top.Rank && r.Score == top.Score)
                .ToList();

            return tiedWinners;
        }
    }
}