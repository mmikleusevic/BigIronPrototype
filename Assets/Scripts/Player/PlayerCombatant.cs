using CombatRoom;
using UnityEngine;

namespace Player
{
    public class PlayerCombatant : Combatant
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Gold gold;
        
        public override Health Health => playerHealth;
        public override Gold Gold => gold;
        
        public void GainGoldAmount(int amount)
        {
            Gold.GainGoldAmount(amount);
        }

        public int LoseGoldAmount(int amount)
        {
            return Gold.LoseGoldAmount(amount);
        }

        public void TakeDamage(int amount)
        {
            Health.TakeDamage(amount);
        }
        
        public void RefreshState()
        {
            Health.RefreshState();
            Gold.RefreshState();
        }
    }
}