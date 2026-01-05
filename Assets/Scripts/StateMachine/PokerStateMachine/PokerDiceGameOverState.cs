using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Managers;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceGameOverState : IState
    {
        private readonly PokerDiceGameController pokerDiceGameController;
        private readonly PokerGame pokerGame;
        private readonly PokerInputs pokerInputs;
        private readonly PokerGameEvents pokerGameEvents;
    
        public PokerDiceGameOverState(PokerDiceGameController controller)
        {
            pokerDiceGameController = controller;
            pokerGame = pokerDiceGameController.PokerGame;
            pokerInputs = pokerDiceGameController.PokerInputs;
            pokerGameEvents = pokerDiceGameController.PokerGameEvents;
        }
    
        public UniTask OnEnter(CancellationToken externalToken)
        {
            pokerGameEvents?.OnGameOverStarted?.Invoke();
            
            pokerInputs.EnablePlayerTurnInput();
            pokerInputs.OnEnd += EndWrapper;

            string winnerResult = "Game ";
            string gameResult = string.Empty;
            
            List<PokerDiceHandResult> winners = DetermineWinners();
            if (winners.Count == 1)
            {
                if (winners[0].PlayerName != GameStrings.PLAYER)
                {
                    winnerResult += "lost!";
                }
                else
                {
                    winnerResult += "won!";
                    if (GameManager.Instance) GameManager.Instance.PlayerCombatant.GainGoldAmount(pokerGame.Wager);
                }
            }
            else
            {
                if (!winners.Select(a => a.PlayerName).Contains(GameStrings.PLAYER)) return UniTask.CompletedTask;

                winnerResult += GameStrings.TIED;
                int numberOfTiedPlayers = winners.Count;
                if (GameManager.Instance) GameManager.Instance.PlayerCombatant.GainGoldAmount(pokerGame.Wager / numberOfTiedPlayers);
            }
            
            gameResult =  winners[0].Description;
            pokerGameEvents?.OnGameOver?.Invoke(winnerResult, gameResult);

            return UniTask.CompletedTask;
        }

        private void EndWrapper()
        {
            End().Forget();
        }
        
        public void OnUpdate()
        {

        }

        public UniTask OnExit()
        {
            pokerInputs.DisablePlayerTurnInput();
            pokerInputs.OnEnd -= EndWrapper;

            if (InputManager.Instance) InputManager.Instance.EnableOnlyUIMap();
            if (GameManager.Instance) GameManager.Instance.RoomPassed();
            
            return UniTask.CompletedTask;
        }

        private async UniTask End()
        {
            pokerInputs.DisablePlayerTurnInput();
            pokerInputs.OnEnd -= EndWrapper;

            if (!LevelManager.Instance) return;

            await LevelManager.Instance.UnloadSceneActivateGame(pokerDiceGameController.PokerAssetReference);
        }
    
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