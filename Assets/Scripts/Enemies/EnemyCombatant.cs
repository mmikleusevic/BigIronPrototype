using System;
using CombatRoom;
using Managers;
using Player;
using Targets;
using UI;
using UnityEngine;
using Weapons;

namespace Enemies
{
    public class EnemyCombatant : Combatant
    {
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private Gold gold;
        
        [field: SerializeField] public TargetSpawner TargetSpawner { get; private set; }
        [field: SerializeField] public TargetProfileSO TargetProfileSO { get; private set; }
        [field: SerializeField] public EnemyUI EnemyUI { get; private set; }
        [field: SerializeField] public int AttackDuration { get; private set; }
        
        public override Health Health => enemyHealth;
        public override Gold Gold => gold;

        public void SpawnTargets()
        {
            TargetSpawner?.SpawnTargets();
        }
        
        public void StopSpawningTargets()
        {
            if (TargetSpawner) TargetSpawner.StopSpawningTargets();
        }

        public void InitializeTargetSpawner(PlayerComboSystem playerComboSystem, Transform middleEnemyTransform)
        {
            TargetSpawner?.Initialize(playerComboSystem, middleEnemyTransform);
        }
    }
}