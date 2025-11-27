using System;
using UnityEngine;

namespace CombatRoom
{
    public class CombatInputRules : MonoBehaviour
    {
        [SerializeField] private CombatRoomManager combatRoomManager;
        
        public bool CanConfirm { get; private set; }
        public bool CanMoveSelection { get; private set; }
        public bool CanShoot { get; private set; }
        public bool CanAim { get; private set; }
        
        private CombatRoomEvents CombatRoomEvents => combatRoomManager.CombatRoomEvents;
        
        private void OnEnable()
        {
            CombatRoomEvents.OnPlayerTurnStarted += SetTargetSelecting;
            CombatRoomEvents.OnPlayerTurnEnded += ResetTargetSelection;
        }

        private void OnDisable()
        {
            CombatRoomEvents.OnPlayerTurnStarted -= SetTargetSelecting;
            CombatRoomEvents.OnPlayerTurnStarted -= ResetTargetSelection;
        }
        
        private void SetTargetSelecting()
        {
            CanConfirm = true;
            CanMoveSelection = true;
        }

        private void ResetTargetSelection()
        {
            CanConfirm = false;
            CanMoveSelection = false;
        }

        private void ResetShooting()
        {
            CanShoot = false;
            CanAim = false;
        }
    }
}