using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatRoom
{
    public class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private Target targetPrefab;
        [SerializeField] private float spawnRadius = 2f;
        [SerializeField] private float spawnDelay = 0.5f;

        private CombatRoomEvents combatRoomEvents;
        private Coroutine spawnTargetsCoroutine;
        private PlayerComboSystem comboSystem;
        private Func<EnemyCombatant> getEnemy;
        
        public void Initialize(CombatRoomEvents events, PlayerComboSystem comboSystem, Func<EnemyCombatant> enemyProvider)
        {
            combatRoomEvents = events;
            this.comboSystem = comboSystem;
            getEnemy = enemyProvider;

            combatRoomEvents.OnPlayerCanAttack += SpawnTargets;
            combatRoomEvents.OnPlayerAttackEnded += StopSpawningTargets;
        }

        private void OnDisable()
        {
            if (combatRoomEvents != null)
            {
                combatRoomEvents.OnPlayerCanAttack -= SpawnTargets;
                combatRoomEvents.OnPlayerAttackEnded -= StopSpawningTargets;
            }

            StopSpawningTargets();
        }

        private void SpawnTargets()
        {
            StopSpawningTargets();
            
            spawnTargetsCoroutine = StartCoroutine(SpawnTargetsCoroutine());
        }

        private void StopSpawningTargets()
        {
            if (spawnTargetsCoroutine != null) StopCoroutine(spawnTargetsCoroutine);
        }

        private IEnumerator SpawnTargetsCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnDelay);

                EnemyCombatant enemy = getEnemy?.Invoke();
                if (!enemy) continue;

                Vector3 origin = enemy.transform.position;
                Vector3 position = origin + Random.insideUnitSphere * spawnRadius;

                Target target = Instantiate(targetPrefab, position, Quaternion.identity);
                
                target.Initialize(enemy.TargetProfileSo);
                
                target.OnTargetHit += comboSystem.OnTargetHitFromSpawner;
                target.OnTargetExpired += comboSystem.OnTargetExpiredFromSpawner;
            }
        }
    }
}