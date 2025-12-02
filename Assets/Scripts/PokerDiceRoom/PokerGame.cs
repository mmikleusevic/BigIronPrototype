using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PokerDiceRoom
{
    public class PokerGame : MonoBehaviour
    {
        public event Action<int> OnWagerChanged;
        
        [field: SerializeField] public PokerPlayer[] Players { get; private set; }
        public Dictionary<PokerPlayer, PokerDiceHandResult> PlayerHands { get; } = new Dictionary<PokerPlayer, PokerDiceHandResult>();
        public Dictionary<PokerPlayer, List<int>> PlayerRolls { get; } = new Dictionary<PokerPlayer, List<int>>();
        public PokerPlayer CurrentPlayer { get; private set; }
        public int Wager { get; private set; }
        
        private int currentPlayerIndex;

        public async UniTask Initialize()
        {
            PlayerHands.Clear();
            SetFirstPlayer();
            SetWager();

            await UniTask.CompletedTask;
        }
        
        public void NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % Players.Length;
            CurrentPlayer = Players[currentPlayerIndex];
        }

        public void SetPlayerHand(PokerPlayer player, PokerDiceHandResult result)
        {
            PlayerHands[player] = result;
        }

        public void SetPlayerRolls(List<int> rolls)
        {
            PlayerRolls[CurrentPlayer] = rolls;
        }
        
        public bool AllPlayersFinished() => PlayerHands.Count >= Players.Length;

        private void SetFirstPlayer()
        {
            int playerIndex = Random.Range(0, Players.Length);
            currentPlayerIndex = playerIndex;
            CurrentPlayer = Players[currentPlayerIndex];
        }

        private void SetWager()
        { 
            Wager = PokerDiceRoomController.Instance.PlayerGoldToWager * Players.Length;
            OnWagerChanged?.Invoke(Wager);
        }
        
        public PokerDiceHandResult GetOpponentBestHand()
        {
            IEnumerable<PokerPlayer> opponents = Players.Where(p => p != CurrentPlayer);

            List<PokerDiceHandResult> opponentHands = new List<PokerDiceHandResult>();

            foreach (PokerPlayer opponent in opponents)
            {
                if (!PlayerRolls.TryGetValue(opponent, out List<int> rolls) || rolls.Count <= 0) continue;
                
                PokerDiceHandResult hand = PokerDiceHandEvaluation.EvaluateHand(opponent, rolls);
                opponentHands.Add(hand);
            }
            
            return opponentHands
                .OrderByDescending(h => h.Rank)
                .ThenByDescending(h => h.Score)
                .First();
        }
    }
}