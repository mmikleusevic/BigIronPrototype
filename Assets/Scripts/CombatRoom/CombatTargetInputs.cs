using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatRoom
{
    public class CombatTargetInputs : MonoBehaviour, ICombatTargetInputSource
    {
        public event Action<Vector2> OnMove;
        public event Action OnConfirm;
        
        [SerializeField] private InputActionAsset inputActionAsset;
        
        private InputActionMap combatTargetMap;
        private InputAction moveAction;
        private InputAction confirmAction;

        private void OnEnable()
        {
            combatTargetMap = inputActionAsset.FindActionMap(GameStrings.COMBAT_TARGET);

            moveAction = combatTargetMap.FindAction(GameStrings.MOVE);
            confirmAction = combatTargetMap.FindAction(GameStrings.CONFIRM);
            
            moveAction.performed += OnMovePerformed;
            confirmAction.performed += OnConfirmPerformed;
            
            DisablePlayerTurnInput();
        }

        private void OnDisable()
        {
            moveAction.performed -= OnMovePerformed;
            confirmAction.performed -= OnConfirmPerformed;
            
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

        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            Vector2 value = ctx.ReadValue<Vector2>();
            OnMove?.Invoke(value);
        }
        
        private void OnConfirmPerformed(InputAction.CallbackContext ctx)
        {
            TriggerConfirm();
        }

        private void TriggerConfirm()
        {
            OnConfirm?.Invoke();
        }
    }
}