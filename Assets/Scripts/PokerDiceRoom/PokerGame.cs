using System.Collections.Generic;
using UnityEngine;

namespace PokerDiceRoom
{
    public class PokerGame : MonoBehaviour
    {
        [field: SerializeField] public string[] Players { get; private set; }
        public Dictionary<string, PokerDiceHandResult> PlayerHands { get; } = new Dictionary<string, PokerDiceHandResult>();
        public Dictionary<string, List<int>> PlayerRolls { get; } = new Dictionary<string, List<int>>();
        public string CurrentPlayer { get; private set; }
        
        private int currentPlayerIndex;

        public void Initialize()
        {
            PlayerHands.Clear();
            SetFirstPlayer();
        }
        
        public void NextPlayer()
        {
            //TODO Check currentPlayer
            currentPlayerIndex = (currentPlayerIndex + 1) % Players.Length;
            CurrentPlayer = Players[currentPlayerIndex];
        }

        public void SetPlayerHand(string playerName, PokerDiceHandResult result)
        {
            PlayerHands[playerName] = result;
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