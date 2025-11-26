using CombatRoom;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyCombatant : Combatant, ICombatantDeathHandler
    {
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private Gold gold;
        
        public override Health Health => enemyHealth;
        public override Gold Gold => gold;
        
        public void PerformAttack()
        {
            // Logic to attack the Player
        }

        public void HandleDeathEffects(Combatant killer)
        {
            if (killer is not PlayerCombatant player) return;
            
            player.Gold.GainGoldAmount(gold.GoldAmount);
        }
    }
}