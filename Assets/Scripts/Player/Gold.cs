using System;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace Player
{
    public class Gold : MonoBehaviour
    {
        public Action<int> OnGoldChanged;
        
        private int goldAmount;
        public int GoldAmount => goldAmount;

        private void OnEnable()
        {
            goldAmount = 0;
        }

        public void GainGoldAmount(int goldGained)
        {
            goldAmount += goldGained;
            OnGoldChanged?.Invoke(GoldAmount);
        }

        public void LoseGoldAmount(int goldLost)
        {
            goldAmount = Mathf.Max(goldAmount - goldLost, 0);
            OnGoldChanged?.Invoke(GoldAmount);
        }

        public void RefreshState()
        {
            OnGoldChanged?.Invoke(goldAmount);
        }
    }
}