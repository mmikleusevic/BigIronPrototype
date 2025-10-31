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
            OnDiceEvaluationStarted?.Invoke();
            
            Debug.Log("=== Evaluating Hand ===");

            foreach (KeyValuePair<string, List<int>> playerRoll in pokerGame.PlayerRolls)
            {
                string playerName = playerRoll.Key;
                
                PokerDiceHandResult result = EvaluateHand(playerName, playerRoll.Value);

                pokerGame.SetPlayerHand(playerName, result);
                
                OnHandEvaluated?.Invoke(result);
            }
            
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
            // Count occurrences of each die value
            Dictionary<int, int> counts = new Dictionary<int, int>();
            foreach (int roll in rolls)
            {
                counts.TryAdd(roll, 0);
                counts[roll]++;
            }
        
            List<int> values = rolls.Select(d => d).OrderBy(v => v).ToList();
            List<int> countValues = counts.Values.OrderByDescending(v => v).ToList();
        
            int totalSum = values.Sum();

            PokerDiceHandResult pokerDiceHandResult = new PokerDiceHandResult(playerName);
            
            // Check for Five of a Kind
            if (countValues[0] == 5)
            {
                pokerDiceHandResult.CreateResult(PokerDiceHandRank.FiveOfAKind, 100, "Five of a Kind!");
                return pokerDiceHandResult;
            }
        
            // Check for Four of a Kind
            if (countValues[0] == 4)
            {
                pokerDiceHandResult.CreateResult(PokerDiceHandRank.FourOfAKind, totalSum + 20, "Four of a Kind!");
                return pokerDiceHandResult;
            }
        
            // Check for Full House (3 of one + 2 of another)
            if (countValues[0] == 3 && countValues[1] == 2)
            {
                pokerDiceHandResult.CreateResult(PokerDiceHandRank.FullHouse, 50 + totalSum, "Full House");
                return pokerDiceHandResult;
            }
        
            // Check for Straight
            bool isStraight = IsStraight(values);
            if (isStraight)
            {
                // Large straight (all 5 consecutive)
                if (values.Count == 5)
                {
                    pokerDiceHandResult.CreateResult(PokerDiceHandRank.Straight, 95, "Straight (Large)");
                    return pokerDiceHandResult;
                }
            }
        
            // Check for Three of a Kind
            if (countValues[0] == 3)
            {
                int threeValue = counts.First(kvp => kvp.Value == 3).Key;
                pokerDiceHandResult.CreateResult(PokerDiceHandRank.ThreeOfAKind, (threeValue * 3) + 10, "Three of a Kind");
                return pokerDiceHandResult;
            }
        
            // Check for Two Pair
            if (countValues[0] == 2 && countValues[1] == 2)
            {
                int pairSum = counts.Where(kvp => kvp.Value == 2).Sum(kvp => kvp.Key * 2);
                pokerDiceHandResult.CreateResult(PokerDiceHandRank.TwoPair, pairSum, "Two Pair");
                return pokerDiceHandResult;
            }
        
            // Check for One Pair
            if (countValues[0] == 2)
            {
                int pairValue = counts.First(kvp => kvp.Value == 2).Key;
                pokerDiceHandResult.CreateResult(PokerDiceHandRank.OnePair, pairValue * 2, "One Pair");
                return pokerDiceHandResult;
            }
        
            // High Card (no hand)
            pokerDiceHandResult.CreateResult(PokerDiceHandRank.HighCard, values.Max(), $"High Card ({values.Max()})");
            return pokerDiceHandResult;
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
    }
}