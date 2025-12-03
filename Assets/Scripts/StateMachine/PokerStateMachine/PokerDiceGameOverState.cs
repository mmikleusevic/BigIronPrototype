using System;
using System.Collections.Generic;
using System.Linq;
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
    
        public UniTask OnEnter()
        {
            pokerGameEvents?.OnGameOverStarted?.Invoke();
            
            pokerInputs.EnablePlayerTurnInput();
            pokerInputs.OnEnd += EndWrapper;
                
            Debug.Log("=== GAME OVER ===");
            
            List<PokerDiceHandResult> winners = DetermineWinners();
            if (winners.Count == 1)
            {
                Debug.Log($"🏆 Winner: {winners[0].PlayerName}");
                
                if (winners[0].PlayerName != GameStrings.PLAYER) return UniTask.CompletedTask;

                if (GameManager.Instance) GameManager.Instance.PlayerCombatant.GainGoldAmount(pokerGame.Wager);
                pokerGameEvents?.OnGameOver?.Invoke();
            }
            else
            {
                string tiedNames = string.Join(", ", winners.Select(w => w.PlayerName));
                Debug.Log($"🤝 It's a tie between {tiedNames}!");
            }

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

            if (LevelManager.Instance)
            {
                LevelManager.Instance.UnloadSceneAsync(pokerDiceGameController.PokerAssetReference.AssetGUID).Forget();
                await LevelManager.Instance.LoadSceneAsync(pokerDiceGameController.GameAssetReference);
            }
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