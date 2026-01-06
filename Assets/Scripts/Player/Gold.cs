using System;
using Managers;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace Player
{
    public class Gold : MonoBehaviour
    {
        public Action<int> OnGoldChanged;

        [SerializeField] private AudioClip coinSound;
        
        [SerializeField] private int goldAmount;
        
        public int GoldAmount => goldAmount;

        public void GainGoldAmount(int goldGained)
        {
            goldAmount += goldGained;

            if (goldGained > 0) SoundManager.Instance.PlayVFX(coinSound);
            
            OnGoldChanged?.Invoke(GoldAmount);
        }

        public int LoseGoldAmount(int goldToLose)
        {
            int actualLoss = Mathf.Min(goldAmount, goldToLose);

            if (actualLoss > 0) SoundManager.Instance.PlayVFX(coinSound);
            
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