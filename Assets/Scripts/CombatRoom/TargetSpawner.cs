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
        [SerializeField] private BaseTarget[] targetPrefabs;
        [SerializeField] private float spawnDelay = 1f;
        
        private Coroutine spawnTargetsCoroutine;
        private PlayerComboSystem comboSystem;
        private EnemyCombatant enemy;

        private void Start()
        {
            enemy = GetComponent<EnemyCombatant>();
        }

        public void Initialize(PlayerComboSystem comboSystem)
        {
            this.comboSystem = comboSystem;
        }

        public void SpawnTargets()
        {
            StopSpawningTargets();
            
            spawnTargetsCoroutine = StartCoroutine(SpawnTargetsCoroutine());
        }

        public void StopSpawningTargets()
        {
            if (spawnTargetsCoroutine == null) return;
            
            StopCoroutine(spawnTargetsCoroutine);
            spawnTargetsCoroutine = null;
        }

        private IEnumerator SpawnTargetsCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnDelay);
                
                if (!enemy) yield break;

                float xOffset = Random.Range(1f, 5f) * (Random.value > 0.5f ? 1f : -1f);
                float zOffset = Random.Range(1f, 3f) * (Random.value > 0.5f ? 1f : -1f);
                float yOffset = Random.Range(1.5f, 5f);
                
                Vector3 spawnOffset = new Vector3(xOffset, yOffset, zOffset);
                Vector3 spawn = enemy.transform.position + spawnOffset;

                MovementAxis axis = (MovementAxis)Random.Range(0, Enum.GetNames(typeof(MovementAxis)).Length);
                TargetSpawnContext targetSpawnContext = new TargetSpawnContext
                {
                    profile = enemy.TargetProfileSO,
                    origin = spawn,
                    movementAxis = axis,
                    centerTransform = enemy.transform 
                };

                BaseTarget prefab = targetPrefabs[Random.Range(0, targetPrefabs.Length)];
                BaseTarget target = Instantiate(prefab, spawn, prefab.transform.rotation);
                target.Initialize(targetSpawnContext);
                
                comboSystem.RegisterTarget(target);
            }
        }
    }
}