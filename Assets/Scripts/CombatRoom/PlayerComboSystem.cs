using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace CombatRoom
{
    public class PlayerComboSystem : MonoBehaviour
    {
        private CombatRoomEvents combatRoomEvents;
        
        public event Action<float> OnComboFinished;
        
        private float DamageMultiplier => hitStreak * 0.5f;
        
        private readonly List<Target> activeTargets = new List<Target>();
        
        private int currentHitStreak;
        private int hitStreak;

        public void Initialize(CombatRoomEvents events)
        {
            combatRoomEvents = events;
            
            combatRoomEvents.OnPlayerAttackStarted += ResetStreaks;
            combatRoomEvents.OnPlayerAttackEnded += FinalizeCombo;
        }

        private void OnDisable()
        {
            combatRoomEvents.OnPlayerAttackStarted -= ResetStreaks;
            combatRoomEvents.OnPlayerAttackEnded -= FinalizeCombo;
            
            UnregisterAndDestroyAllTargets();
        }

        public void RegisterTarget(Target target)
        {
            target.OnTargetHit += OnTargetHit;
            activeTargets.Add(target);
        }

        private void OnTargetHit(Target target)
        {
            target.OnTargetHit -= OnTargetHit;
            currentHitStreak++;

            if (currentHitStreak > hitStreak) hitStreak = currentHitStreak;

            activeTargets.Remove(target);
        }

        public void OnMiss()
        {
            currentHitStreak = 0;
        }

        private void ResetStreaks()
        {
            currentHitStreak = 0;
            hitStreak = 0;
        }
        
        private void FinalizeCombo()
        {
            OnComboFinished?.Invoke(DamageMultiplier);
            UnregisterAndDestroyAllTargets();
        }

        private void UnregisterAndDestroyAllTargets()
        {
            foreach (Target target in activeTargets)
            {
                target.OnTargetHit -= OnTargetHit;
                Destroy(target.gameObject);
            }
            
            activeTargets.Clear();
        }
    }
}