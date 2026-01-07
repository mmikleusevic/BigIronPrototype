using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatRoom
{
    public class CombatTargetInputs : MonoBehaviour, ICombatTargetInputSource
    {
        private const float MOVE_TRIGGER_THRESHOLD = 0.3f;
        private const float MOVE_RESET_THRESHOLD = 0.2f;
        
        public event Action OnShootSelected;
        public event Action<float> OnMove;
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
        
        private bool canMove = true;

        private void OnEnable()
        {
            combatTargetMap = inputActionAsset.FindActionMap(GameStrings.COMBAT_TARGET);

            shootSelectedAction = combatTargetMap.FindAction(GameStrings.SHOOT_SELECT);
            moveAction = combatTargetMap.FindAction(GameStrings.MOVE);
            confirmAction = combatTargetMap.FindAction(GameStrings.CONFIRM);
            cancelAction = combatTargetMap.FindAction(GameStrings.CANCEL);

            shootSelectedAction.performed += OnShootSelectPerformed;
            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMoveCanceled;
            confirmAction.performed += OnConfirmPerformed;
            cancelAction.performed += OnCancelPerformed;
            
            DisablePlayerTurnInput();
        }

        private void OnDisable()
        {
            shootSelectedAction.performed -= OnShootSelectPerformed;
            moveAction.performed -= OnMovePerformed;
            moveAction.canceled -= OnMoveCanceled;
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

            float input = ctx.ReadValue<float>();
            
            if (canMove)
            {
                if (input > MOVE_TRIGGER_THRESHOLD)
                {
                    OnMove?.Invoke(1);
                    canMove = false;
                }
                else if (input < -MOVE_TRIGGER_THRESHOLD)
                {
                    OnMove?.Invoke(-1);
                    canMove = false;
                }
            }
            else
            {
                if (Mathf.Abs(input) < MOVE_RESET_THRESHOLD)
                {
                    canMove = true;
                }
            }
        }
        
        private void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            canMove = true;
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