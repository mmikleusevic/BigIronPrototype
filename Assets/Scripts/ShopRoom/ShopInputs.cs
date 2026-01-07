using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShopRoom
{
    public class ShopInputs : MonoBehaviour
    {
        private const float MOVE_TRIGGER_THRESHOLD = 0.3f;
        private const float MOVE_RESET_THRESHOLD = 0.2f;
        
        public event Action<float> OnMove;
        public event Action OnConfirm;
        public event Action OnLeave;

        [Header("Input Action References")] 
		[SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private ShopController shopController;
        
        private InputActionMap shopMap;
        private InputAction moveAction;
		private InputAction confirmAction;
		private InputAction leaveAction;
        
        private bool canMove = true;

        private void OnEnable()
        {
            shopMap = inputActionAsset.FindActionMap(GameStrings.SHOP);
            
            moveAction = shopMap.FindAction(GameStrings.MOVE);
            confirmAction = shopMap.FindAction(GameStrings.CONFIRM);
            leaveAction = shopMap.FindAction(GameStrings.LEAVE);
            
            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMoveCanceled;
            confirmAction.performed += OnConfirmPerformed;
            leaveAction.performed += OnLeavePerformed;

            EnableInputs();
        }

        private void OnDisable()
        {
            moveAction.performed -= OnMovePerformed;
            moveAction.canceled -= OnMoveCanceled;
            confirmAction.performed -= OnConfirmPerformed;
            leaveAction.performed -= OnLeavePerformed;
            
            DisableInputs();
        }
        
        private void EnableInputs()
        {
            shopMap.Enable();
        }
        
        private void DisableInputs()
        {
            shopMap.Disable();
        }
        
        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
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
            OnConfirm?.Invoke();
        }

        private void OnLeavePerformed(InputAction.CallbackContext ctx)
        {
            OnLeave?.Invoke();
        }
    }
}