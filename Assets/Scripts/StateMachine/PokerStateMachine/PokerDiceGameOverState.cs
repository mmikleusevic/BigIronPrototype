using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Managers;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceGameOverState : IPokerDiceState
    {
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly PokerGame pokerGame;
        private readonly PokerInputs pokerInputs;
        private readonly PokerGameEvents pokerGameEvents;
    
        public PokerDiceGameOverState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            pokerGame = pokerDiceGameManager.PokerGame;
            pokerInputs = pokerDiceGameManager.PokerInputs;
            pokerGameEvents = pokerDiceGameManager.PokerGameEvents;
        }
    
        public void OnEnter()
        {
            pokerGameEvents.OnGameOverStarted?.Invoke();
            
            pokerInputs.OnEnd += EndWrapper;
                
            Debug.Log("=== GAME OVER ===");
            
            List<PokerDiceHandResult> winners = DetermineWinners();
            if (winners.Count == 1)
            {
                Debug.Log($"🏆 Winner: {winners[0].PlayerName}");
                
                if (winners[0].PlayerName != GameStrings.PLAYER) return;
                
                GameManager.Instance.PlayerContext.GainGoldAmount(pokerGame.Wager);
                pokerGameEvents.OnGameOver?.Invoke();
            }
            else
            {
                string tiedNames = string.Join(", ", winners.Select(w => w.PlayerName));
                Debug.Log($"🤝 It's a tie between {tiedNames}!");
            }
        }

        private void EndWrapper()
        {
            _ = End();
        }

        private async UniTask End()
        {
            pokerInputs.OnEnd -= EndWrapper;
            
            _ = LevelManager.Instance.UnloadSceneAsync(pokerDiceGameManager.PokerAssetReference.AssetGUID);
            await LevelManager.Instance.LoadSceneAsync(pokerDiceGameManager.GameAssetReference);
        }
    
        public void OnUpdate() { }

        public void OnExit()
        {
            pokerInputs.OnEnd -= EndWrapper;
            
            InputManager.Instance.EnableOnlyUIMap();
            GameManager.Instance.RoomPassed();
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