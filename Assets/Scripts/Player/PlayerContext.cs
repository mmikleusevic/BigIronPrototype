using System;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace Player
{
    public class PlayerContext : MonoBehaviour
    {
        public event Action<int> OnGoldChanged;
        public event Action<int> OnHpChanged;
        public event Action OnPlayerDeath;
    
        [SerializeField] private int MaxHealth;
        public int CurrentHealth { get; private set; }
        public int Gold { get; private set; }

        private void OnEnable()
        {
            PokerDiceGameOverState.OnGameOver += GainGoldAmount;
            
            CurrentHealth = MaxHealth;
            Gold = 10;
        }

        private void OnDisable()
        {
            PokerDiceGameOverState.OnGameOver -= GainGoldAmount;
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
            CurrentHealth = Mathf.Clamp(CurrentHealth - amount,0,MaxHealth);
            OnHpChanged?.Invoke(CurrentHealth);

            if (CurrentHealth == 0) OnPlayerDeath?.Invoke();
        }
    }
}
