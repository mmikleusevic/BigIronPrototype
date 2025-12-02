using System;
using CombatRoom;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyCombatant : Combatant, ICombatantDeathHandler
    {
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private Gold gold;
        [field: SerializeField] public EnemyUI EnemyUI { get; private set; }
        
        public override Health Health => enemyHealth;
        public override Gold Gold => gold;

        public void PerformAttack()
        {
            // Logic to attack the Player
        }

        public void SelectEnemy()
        {
            EnemyUI.Show();
        }

        public void HandleDeathEffects(Combatant killer)
        {
            if (killer is not PlayerCombatant player) return;
            
            player.Gold.GainGoldAmount(gold.GoldAmount);
        }
    }
}