using System;
using System.Collections;
using System.Collections.Generic;
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
        private Func<Vector3> getTargetPosition;
        
        public void Initialize(CombatRoomEvents events, PlayerComboSystem comboSys, Func<Vector3> positionProvider)
        {
            combatRoomEvents = events;
            comboSystem = comboSys;
            getTargetPosition = positionProvider;

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

                if (getTargetPosition == null) continue;
                
                Vector3 origin = getTargetPosition.Invoke();
                Vector3 position = origin + Random.insideUnitSphere * spawnRadius;

                Target target = Instantiate(targetPrefab, position, Quaternion.identity);
                
                comboSystem.RegisterTarget(target);
            }
        }
    }
}