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
            
            PokerDiceGameOverState.OnGameOver += GainGoldAmount;
        }

        private void OnDisable()
        {
            PokerDiceGameOverState.OnGameOver -= GainGoldAmount;
        }

        public void GainGoldAmount(int goldGained)
        {
            goldAmount += goldGained;
            OnGoldChanged?.Invoke(GoldAmount);
        }

        public void LoseGoldAmount(int goldLost)
        {
            goldAmount -= goldLost;
            OnGoldChanged?.Invoke(GoldAmount);
        }
    }
}