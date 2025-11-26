using System;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace Player
{
    public class Gold : MonoBehaviour
    {
        public Action<int> OnGoldChanged;

        [SerializeField] private int goldAmount;
        
        public int GoldAmount => goldAmount;

        public void GainGoldAmount(int goldGained)
        {
            goldAmount += goldGained;
            OnGoldChanged?.Invoke(GoldAmount);
        }

        public int LoseGoldAmount(int goldToLose)
        {
            int actualLoss = Mathf.Min(goldAmount, goldToLose);

            goldAmount -= actualLoss;
            OnGoldChanged?.Invoke(GoldAmount);

            return actualLoss;
        }

        public void RefreshState()
        {
            OnGoldChanged?.Invoke(goldAmount);
        }
    }
}