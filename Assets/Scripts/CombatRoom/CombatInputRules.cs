using System;
using UnityEngine;

namespace CombatRoom
{
    public class CombatInputRules : MonoBehaviour
    {
        [SerializeField] private CombatRoomManager combatRoomManager;
        
        public bool CanConfirm { get; private set; }
        public bool CanMoveSelection { get; private set; }
        public bool CanSelectShoot { get; private set; }
        public bool CanShoot { get; private set; }
        public bool CanAim { get; private set; }
        public bool CanCancel { get; private set; }
        
        private CombatRoomEvents CombatRoomEvents => combatRoomManager.CombatRoomEvents;
        
        private void OnEnable()
        {
            CombatRoomEvents.OnPlayerTurnStarted += SetPlayerTurnState;
            CombatRoomEvents.OnPlayerTurnEnded += ResetPlayerTurnState;
            CombatRoomEvents.OnPlayerTargetSelectingStarted += SetTargetSelecting;
            CombatRoomEvents.OnPlayerTargetSelectingEnded += ResetTargetSelection;
        }

        private void OnDisable()
        {
            CombatRoomEvents.OnPlayerTurnStarted -= SetPlayerTurnState;
            CombatRoomEvents.OnPlayerTurnEnded -= ResetPlayerTurnState;
            CombatRoomEvents.OnPlayerTargetSelectingStarted -= SetTargetSelecting;
            CombatRoomEvents.OnPlayerTargetSelectingEnded -= ResetTargetSelection;
        }

        private void SetPlayerTurnState()
        {
            CanSelectShoot = true;
        }

        private void ResetPlayerTurnState()
        {
            CanSelectShoot = false;
        }
        
        private void SetTargetSelecting()
        {
            CanConfirm = true;
            CanMoveSelection = true;
            CanCancel = true;
        }

        private void ResetTargetSelection()
        {
            CanConfirm = false;
            CanMoveSelection = false;
            CanCancel = false;
        }

        private void ResetShooting()
        {
            CanShoot = false;
            CanAim = false;
        }
    }
}