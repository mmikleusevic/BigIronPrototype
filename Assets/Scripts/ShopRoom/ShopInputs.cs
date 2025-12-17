using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShopRoom
{
    public class ShopInputs : MonoBehaviour
    {
        public event Action<Vector2> OnMove;
        public event Action OnConfirm;
        public event Action OnLeave;

        [Header("Input Action References")] 
		[SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private ShopController shopController;
        
        private InputActionMap shopMap;
        private InputAction moveAction;
		private InputAction confirmAction;
		private InputAction leaveAction;

        private void OnEnable()
        {
            shopMap = inputActionAsset.FindActionMap(GameStrings.SHOP);
            
            moveAction = shopMap.FindAction(GameStrings.MOVE);
            confirmAction = shopMap.FindAction(GameStrings.CONFIRM);
            leaveAction = shopMap.FindAction(GameStrings.LEAVE);
            
            moveAction.performed += OnMovePerformed;
            confirmAction.performed += OnConfirmPerformed;
            leaveAction.performed += OnLeavePerformed;

            EnableInputs();
        }

        private void OnDisable()
        {
            moveAction.performed -= OnMovePerformed;
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
            Vector2 value = ctx.ReadValue<Vector2>();
            OnMove?.Invoke(value);
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