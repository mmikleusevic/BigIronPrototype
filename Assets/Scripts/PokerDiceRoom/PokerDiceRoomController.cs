using System;
using UnityEngine;

namespace PokerDiceRoom
{
    public class PokerDiceRoomController : MonoBehaviour
    {
        public event Action OnPokerDiceRoomPressed;
        
        public static PokerDiceRoomController Instance { get; private set; }

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