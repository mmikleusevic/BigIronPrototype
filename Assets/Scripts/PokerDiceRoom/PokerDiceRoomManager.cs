using System;
using UnityEngine;

namespace PokerDiceRoom
{
    public class PokerDiceRoomManager : MonoBehaviour
    {
        public event Action OnPokerDiceRoomPressed;
        
        public static PokerDiceRoomManager Instance { get; private set; }

        public int PlayerGoldToWager { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        public void DisplayPokerDice()
        {
            OnPokerDiceRoomPressed?.Invoke();    
        }

        public void SetWager(int gold)
        {
            PlayerGoldToWager = gold;
        }
    }
}