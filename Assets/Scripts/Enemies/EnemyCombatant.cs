using System;
using CombatRoom;
using Managers;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyCombatant : Combatant
    {
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private Gold gold;
        
        [field: SerializeField] public TargetSpawner TargetSpawner { get; private set; }
        [field: SerializeField] public TargetProfileSO TargetProfileSO { get; private set; }
        [field: SerializeField] public EnemyUI EnemyUI { get; private set; }
        
        public override Health Health => enemyHealth;
        public override Gold Gold => gold;

        public void Attack(PlayerCombatant playerCombatant)
        {
            playerCombatant.TakeDamage(this, playerCombatant, Data.damage);
        }

        public void SpawnTargets()
        {
            TargetSpawner?.SpawnTargets();
        }
        
        public void StopSpawningTargets()
        {
            TargetSpawner?.StopSpawningTargets();
        }

        public void InitializeTargetSpawner(PlayerComboSystem playerComboSystem)
        {
            TargetSpawner?.Initialize(playerComboSystem);
        }
    }
}