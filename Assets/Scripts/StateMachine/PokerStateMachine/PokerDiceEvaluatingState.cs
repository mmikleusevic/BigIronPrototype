using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceEvaluatingState : IState
    {
        private readonly PokerDiceGameController pokerDiceGameController;
        private readonly PokerGame pokerGame;
        private readonly PokerGameEvents pokerGameEvents;
    
        private readonly float displayDuration = 3f;
        private float displayTimer;
    
        public PokerDiceEvaluatingState(PokerDiceGameController controller)
        {
            pokerDiceGameController = controller;
            pokerGame = pokerDiceGameController.PokerGame;
            pokerGameEvents = pokerDiceGameController.PokerGameEvents;
        }
    
        public async UniTask OnEnter()
        {
            pokerGameEvents.OnDiceEvaluationStarted?.Invoke();
            Debug.Log("=== Evaluating Hands ===");

            Dictionary<PokerPlayer, PokerDiceHandResult> results = new();

            foreach ((PokerPlayer playerName, List<int> rolls) in pokerGame.PlayerRolls)
            {
                PokerDiceHandResult result = PokerDiceHandEvaluation.EvaluateHand(playerName, rolls);
                pokerGame.SetPlayerHand(playerName, result);
                results[playerName] = result;
            }

            List<PokerDiceHandResult> winners = DetermineWinners(results);
            DisplayEvaluationReport(results, winners);

            await UniTask.Delay(TimeSpan.FromSeconds(displayDuration));
            
            if (pokerGame.AllPlayersFinished())
            {
                await pokerDiceGameController.BaseStateMachine.ChangeState(new PokerDiceGameOverState(pokerDiceGameController));
            }
        }
    
        public void OnUpdate()
        {

        }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }

        // TODO remove methods later
        private void DisplayEvaluationReport(Dictionary<PokerPlayer, PokerDiceHandResult> results, List<PokerDiceHandResult> winners)
        {
            Debug.Log("=== Hand Evaluation Results ===");

            foreach ((PokerPlayer player, PokerDiceHandResult result) in results)
            {
                Debug.Log($"{player}: {result.Description} (Score: {result.Score})");
            }

            if (winners.Count == 1)
            {
                // One winner
                PokerDiceHandResult winner = winners[0];
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