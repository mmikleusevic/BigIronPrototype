using System;
using UnityEngine;

namespace Managers
{
    public class PokerDiceRoomManager : MonoBehaviour
    {
        public static PokerDiceRoomManager Instance { get; private set; }
        public event Action OnPokerDiceRoomPressed;
        public event Action OnPokerDiceRoomLoaded;
        
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
        
        public void PokerDiceRoomLoaded()
        {
            OnPokerDiceRoomLoaded?.Invoke();
        }
    }
}