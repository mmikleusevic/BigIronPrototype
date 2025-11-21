using System;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace Player
{
    public class PlayerContext : MonoBehaviour
    {
        [field: SerializeField] public Gold Gold { get; private set; }
        [field: SerializeField] public PlayerHealth PlayerHealth { get; private set; }

        public void GainGoldAmount(int amount)
        {
            Gold.GainGoldAmount(amount);
        }

        public void LoseGoldAmount(int amount)
        {
            Gold.LoseGoldAmount(amount);
        }

        public void TakeDamage(int amount)
        {
            PlayerHealth.TakeDamage(amount);
        }
    }
}