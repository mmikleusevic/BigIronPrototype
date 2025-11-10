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

            Dictionary<string, PokerDiceHandResult> results = new();

            foreach (var (playerName, rolls) in pokerGame.PlayerRolls)
            {
                PokerDiceHandResult result = EvaluateHand(playerName, rolls);
                pokerGame.SetPlayerHand(playerName, result);
                results[playerName] = result;
                OnHandEvaluated?.Invoke(result);
            }

            PokerDiceHandResult winner = DetermineWinner(results);
            DisplayEvaluationReport(results, winner);

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
    
        // Poker hand evaluation logic
        private PokerDiceHandResult EvaluateHand(string playerName, List<int> rolls)
        {
            Dictionary<int, int> counts = rolls.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
            List<int> sorted = rolls.OrderBy(v => v).ToList();
            List<int> frequencies = counts.Values.OrderByDescending(v => v).ToList();
            int sum = sorted.Sum();

            var result = new PokerDiceHandResult(playerName);

            if (frequencies[0] == 5)
                return result.CreateResult(PokerDiceHandRank.FiveOfAKind, 100, "Five of a Kind!");

            if (frequencies[0] == 4)
                return result.CreateResult(PokerDiceHandRank.FourOfAKind, sum + 20, "Four of a Kind!");

            if (frequencies[0] == 3 && frequencies[1] == 2)
                return result.CreateResult(PokerDiceHandRank.FullHouse, sum + 50, "Full House!");

            if (IsStraight(sorted))
                return result.CreateResult(PokerDiceHandRank.Straight, 95, "Straight");

            if (frequencies[0] == 3)
            {
                int value = counts.First(kvp => kvp.Value == 3).Key;
                return result.CreateResult(PokerDiceHandRank.ThreeOfAKind, value * 3 + 10, "Three of a Kind");
            }

            if (frequencies.Count(f => f == 2) == 2)
            {
                int pairSum = counts.Where(kvp => kvp.Value == 2).Sum(kvp => kvp.Key * 2);
                return result.CreateResult(PokerDiceHandRank.TwoPair, pairSum, "Two Pair");
            }

            if (frequencies[0] == 2)
            {
                int value = counts.First(kvp => kvp.Value == 2).Key;
                return result.CreateResult(PokerDiceHandRank.OnePair, value * 2, "One Pair");
            }

            int highCard = sorted.Max();
            return result.CreateResult(PokerDiceHandRank.HighCard, highCard, $"High Card ({highCard})");
        }
    
        private bool IsStraight(List<int> sortedValues)
        {
            // Check for 1,2,3,4,5 or 2,3,4,5,6
            if (sortedValues.Count != 5) return false;
        
            for (int i = 0; i < sortedValues.Count - 1; i++)
            {
                if (sortedValues[i + 1] != sortedValues[i] + 1) return false;
            }
        
            return true;
        }
        
        private void DisplayEvaluationReport(Dictionary<string, PokerDiceHandResult> results, PokerDiceHandResult winner)
        {
            Debug.Log("=== Hand Evaluation Results ===");

            foreach (var (player, result) in results)
            {
                Debug.Log($"{player}: {result.Description} (Score: {result.Score})");
            }

            Debug.Log($"🏆 Winner: {winner.PlayerName} with {winner.Description}!");
        }
        
        private PokerDiceHandResult DetermineWinner(Dictionary<string, PokerDiceHandResult> results)
        {
            // Order by hand rank first, then by score (for tie-breaker)
            return results
                .Values
                .OrderByDescending(r => r.Rank)
                .ThenByDescending(r => r.Score)
                .First();
        }
    }
}