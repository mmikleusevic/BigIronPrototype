using System.Collections.Generic;
using UnityEngine;

namespace PokerDiceRoom
{
    public class PokerGame : MonoBehaviour
    {
        [field: SerializeField] public PokerPlayer[] Players { get; private set; }
        public Dictionary<PokerPlayer, PokerDiceHandResult> PlayerHands { get; } = new Dictionary<PokerPlayer, PokerDiceHandResult>();
        public Dictionary<PokerPlayer, List<int>> PlayerRolls { get; } = new Dictionary<PokerPlayer, List<int>>();
        public PokerPlayer CurrentPlayer { get; private set; }
        
        private int currentPlayerIndex;

        public void Initialize()
        {
            PlayerHands.Clear();
            SetFirstPlayer();
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
    }
}