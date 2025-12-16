using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    
        public async UniTask OnEnter(CancellationToken externalToken)
        {
            pokerGameEvents?.OnDiceEvaluationStarted?.Invoke();

            Dictionary<PokerPlayer, PokerDiceHandResult> results = new();

            foreach ((PokerPlayer playerName, List<int> rolls) in pokerGame.PlayerRolls)
            {
                PokerDiceHandResult result = PokerDiceHandEvaluation.EvaluateHand(playerName, rolls);
                pokerGame.SetPlayerHand(playerName, result);
                results[playerName] = result;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(displayDuration), cancellationToken: externalToken);
            
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
        
        private List<PokerDiceHandResult> DetermineWinners(Dictionary<PokerPlayer, PokerDiceHandResult> results)
        {
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