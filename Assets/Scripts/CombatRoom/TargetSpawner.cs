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
                
                transform.localScale = Vector3.one * enemy.TargetProfileSO.scale;
                
                float minAngle = Mathf.PI * 0.25f;
                float maxAngle = Mathf.PI * 1.75f;
                
                float orbitAngle = Random.Range(minAngle, maxAngle); 

                float x = Mathf.Cos(orbitAngle) * enemy.TargetProfileSO.orbitRadius;
                float z = Mathf.Sin(orbitAngle) * enemy.TargetProfileSO.orbitRadius;
                float y = Mathf.Sin(orbitAngle * enemy.TargetProfileSO.speed) * 0.5f;
        
                Vector3 origin = enemy.transform.position;
                Vector3 startPosition = origin + new Vector3(x, y, z);
        
                Target target = Instantiate(targetPrefab, startPosition, Quaternion.identity);
                target.Initialize(enemy.TargetProfileSO, origin, orbitAngle);
        
                comboSystem.RegisterTarget(target);
            }
        }
    }
}