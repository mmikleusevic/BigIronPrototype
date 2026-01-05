using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatRoom
{
    public class CombatTargetInputs : MonoBehaviour, ICombatTargetInputSource
    {
        public event Action OnShootSelected;
        public event Action<Vector2> OnMove;
        public event Action OnConfirm;
        public event Action OnCancel;
        
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private CombatRoomController combatRoomController;
        
        private InputActionMap combatTargetMap;
        private InputAction shootSelectedAction;
        private InputAction moveAction;
        private InputAction confirmAction;
        private InputAction cancelAction;
        
        private CombatInputRules CombatInputRules => combatRoomController.CombatInputRules; 

        private void OnEnable()
        {
            combatTargetMap = inputActionAsset.FindActionMap(GameStrings.COMBAT_TARGET);

            shootSelectedAction = combatTargetMap.FindAction(GameStrings.SHOOT_SELECT);
            moveAction = combatTargetMap.FindAction(GameStrings.MOVE);
            confirmAction = combatTargetMap.FindAction(GameStrings.CONFIRM);
            cancelAction = combatTargetMap.FindAction(GameStrings.CANCEL);

            shootSelectedAction.performed += OnShootSelectPerformed;
            moveAction.performed += OnMovePerformed;
            confirmAction.performed += OnConfirmPerformed;
            cancelAction.performed += OnCancelPerformed;
            
            DisablePlayerTurnInput();
        }

        private void OnDisable()
        {
            shootSelectedAction.performed -= OnShootSelectPerformed;
            moveAction.performed -= OnMovePerformed;
            confirmAction.performed -= OnConfirmPerformed;
            cancelAction.performed -= OnCancelPerformed;
            
            DisablePlayerTurnInput();
        }
        
        public void EnablePlayerTurnInput()
        {
            combatTargetMap.Enable();
        }
        
        public void DisablePlayerTurnInput()
        {
            combatTargetMap.Disable();
        }
        
        private void OnShootSelectPerformed(InputAction.CallbackContext ctx)
        {
            TriggerShootSelected();
        }

        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            if (!CombatInputRules.CanMoveSelection) return;
            
            Vector2 value = ctx.ReadValue<Vector2>();
            OnMove?.Invoke(value);
        }
        
        private void OnConfirmPerformed(InputAction.CallbackContext ctx)
        {
            TriggerConfirm();
        }
        
        private void OnCancelPerformed(InputAction.CallbackContext ctx)
        {
            TriggerCancel();
        }

        public void TriggerShootSelected()
        {
            if (!CombatInputRules.CanSelectShoot) return;
            
            OnShootSelected?.Invoke();
        }

        public void TriggerConfirm()
        {
            if (!CombatInputRules.CanConfirm) return;
            
            OnConfirm?.Invoke();
        }
        
        public void TriggerCancel()
        {
            if (!CombatInputRules.CanCancel) return;
            
            OnCancel?.Invoke();
        }
    }
}