using System;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace Player
{
    public class PlayerContext : MonoBehaviour
    {
        public event Action<int> OnGoldChanged;
        public event Action<int, int> OnHealthChanged;
        public event Action OnPlayerDeath;

        [SerializeField] private int maxHealth;
        public int CurrentHealth { get; private set; }
        public int Gold { get; private set; }

        private void OnEnable()
        {
            CurrentHealth = maxHealth;
            Gold = 0;
        }

        public void GainGoldAmount(int amount)
        {
            Gold += amount;
            OnGoldChanged?.Invoke(Gold);
        }

        public void LoseGoldAmount(int amount)
        {
            Gold -= amount;
            OnGoldChanged?.Invoke(Gold);
        }

        public void LoseHealth(int amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - amount,0,maxHealth);
            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

            if (CurrentHealth == 0) OnPlayerDeath?.Invoke();
        }

        public void RefreshState()
        {
            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
            OnGoldChanged?.Invoke(Gold);
        }
    }
}
