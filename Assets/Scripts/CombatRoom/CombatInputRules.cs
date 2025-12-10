using System;
using Managers;
using UnityEngine;

namespace CombatRoom
{
    public class CombatInputRules : MonoBehaviour
    {
        [SerializeField] private CombatRoomController combatRoomController;
        
        public bool CanConfirm { get; private set; }
        public bool CanMoveSelection { get; private set; }
        public bool CanSelectShoot { get; private set; }
        public bool CanShoot { get; private set; }
        public bool CanAim { get; private set; }
        public bool CanCancel { get; private set; }
        public bool CanReload { get; private set; }
        public bool CanEnd { get; private set; }
        
        private CombatRoomEvents CombatRoomEvents => combatRoomController.CombatRoomEvents;
        
        private void OnEnable()
        {
            CombatRoomEvents.OnPlayerTurnStarted += SetPlayerTurnState;
            CombatRoomEvents.OnPlayerTurnEnded += Reset;
            CombatRoomEvents.OnPlayerTargetSelectingStarted += SetTargetSelecting;
            CombatRoomEvents.OnPlayerTargetSelectingEnded += Reset;
            CombatRoomEvents.OnPlayerAttackStarted += SetPlayerAttack;
            CombatRoomEvents.OnPlayerAttackEnded += Reset;
            CombatRoomEvents.OnVictoryStarted += SetPlayerWon;
        }

        private void OnDisable()
        {
            CombatRoomEvents.OnPlayerTurnStarted -= SetPlayerTurnState;
            CombatRoomEvents.OnPlayerTurnEnded -= Reset;
            CombatRoomEvents.OnPlayerTargetSelectingStarted -= SetTargetSelecting;
            CombatRoomEvents.OnPlayerTargetSelectingEnded -= Reset;
            CombatRoomEvents.OnPlayerAttackStarted -= SetPlayerAttack;
            CombatRoomEvents.OnPlayerAttackEnded -= Reset;
            CombatRoomEvents.OnVictoryStarted += SetPlayerWon;
        }

        private void SetPlayerTurnState()
        {
            CanConfirm = false;
            CanMoveSelection = false;
            CanSelectShoot = true;
            CanShoot = false;
            CanAim = false;
            CanCancel = false;
            CanReload = false;
            CanEnd = false;
        }
        
        private void SetTargetSelecting()
        {
            CanConfirm = true;
            CanMoveSelection = true;
            CanEnd = false;
            CanAim = false;
            CanReload = false;
            CanShoot = false;
            CanSelectShoot = false;
            CanCancel = true;
        }

        private void SetPlayerAttack()
        {
            CanConfirm = false;
            CanMoveSelection = false;
            CanEnd = false;
            CanAim = true;
            CanReload = true;
            CanShoot = true;
            CanSelectShoot = false;
            CanCancel = false;
        }

        private void SetPlayerWon()
        {
            CanConfirm = false;
            CanMoveSelection = false;
            CanEnd = true;
            CanAim = false;
            CanReload = false;
            CanShoot = false;
            CanSelectShoot = false;
            CanCancel = false;
        }

        private void Reset()
        {
            CanConfirm = false;
            CanMoveSelection = false;
            CanEnd = false;
            CanAim = false;
            CanReload = false;
            CanShoot = false;
            CanSelectShoot = false;
            CanCancel = false;
        }
    }
}