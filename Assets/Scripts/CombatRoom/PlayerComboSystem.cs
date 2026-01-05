using System;
using System.Collections.Generic;
using Player;
using Targets;
using UnityEngine;

namespace CombatRoom
{
    public class PlayerComboSystem : MonoBehaviour
    {
        [SerializeField] private Combo comboPrefab;
        
        private CombatRoomEvents combatRoomEvents;
        
        public event Action<int> OnNewHitStreak;
        
        public float DamageMultiplier => hitStreak * 0.5f;
        
        private readonly List<BaseTarget> activeTargets = new List<BaseTarget>();
        
        private int currentHitStreak;
        private int hitStreak;

        public void Initialize(CombatRoomEvents events)
        {
            combatRoomEvents = events;
            
            if (combatRoomEvents == null) return;
            
            combatRoomEvents.OnPlayerAttackStarted += ResetStreaks;
            combatRoomEvents.OnPlayerAttackEnded += DestroyTargets;
        }

        private void OnDisable()
        {
            if (combatRoomEvents == null) return;
            
            combatRoomEvents.OnPlayerAttackStarted -= ResetStreaks;
            combatRoomEvents.OnPlayerAttackEnded -= DestroyTargets;
            
            UnregisterAndDestroyAllTargets();
        }

        public void RegisterTarget(BaseTarget baseTarget)
        {
            activeTargets.Add(baseTarget);
            baseTarget.OnTargetHit += OnTargetHitFromSpawner;
            baseTarget.OnTargetExpired += OnTargetExpiredFromSpawner;
        }

        private void OnTargetHit(BaseTarget baseTarget)
        {
            baseTarget.OnTargetHit -= OnTargetHit;
            currentHitStreak++;

            if (currentHitStreak > hitStreak)
            {
                hitStreak = currentHitStreak;
                OnNewHitStreak?.Invoke(hitStreak);
            }

            activeTargets.Remove(baseTarget);
        }

        private void OnMiss()
        {
            currentHitStreak = 0;
        }

        private void ResetStreaks()
        {
            currentHitStreak = 0;
            hitStreak = 0;
        }
        
        private void DestroyTargets()
        {
            UnregisterAndDestroyAllTargets();
        }

        private void UnregisterAndDestroyAllTargets()
        {
            foreach (BaseTarget baseTarget in activeTargets)
            {
                if (!baseTarget) continue;
                
                baseTarget.OnTargetHit -= OnTargetHit;
                Destroy(baseTarget.gameObject);
            }
            
            activeTargets.Clear();
        }
        
        private void OnTargetHitFromSpawner(BaseTarget baseTarget)
        {
            baseTarget.OnTargetHit -= OnTargetHitFromSpawner;
            baseTarget.OnTargetExpired -= OnTargetExpiredFromSpawner;

            currentHitStreak++;

            Combo combo = Instantiate(comboPrefab, baseTarget.transform.position, Quaternion.identity);
            combo.Initialize(currentHitStreak);

            if (currentHitStreak > hitStreak)
            {
                hitStreak = currentHitStreak;
                OnNewHitStreak?.Invoke(hitStreak); 
            }

            activeTargets.Remove(baseTarget);
        }

        private void OnTargetExpiredFromSpawner(BaseTarget baseTarget)
        {
            baseTarget.OnTargetHit -= OnTargetHitFromSpawner;
            baseTarget.OnTargetExpired -= OnTargetExpiredFromSpawner;
            
            OnMiss();

            activeTargets.Remove(baseTarget);
        }
    }
}