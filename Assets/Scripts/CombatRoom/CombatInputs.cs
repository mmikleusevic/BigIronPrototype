using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatRoom
{
    public class CombatInputs : MonoBehaviour, ICombatInputSource
    {
        public event Action OnShoot;
        public event Action<Vector2> OnAim;

        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private CombatRoomManager combatRoomManager;

        private InputActionMap combatMap;
        private InputAction shootAction;
        private InputAction aimAction;

        private CombatInputRules CombatInputRules => combatRoomManager.CombatInputRules;

        private void OnEnable()
        {
            combatMap = inputActionAsset.FindActionMap(GameStrings.COMBAT);

            shootAction = combatMap.FindAction(GameStrings.SHOOT);
            aimAction = combatMap.FindAction(GameStrings.AIM);

            shootAction.performed += OnShootPerformed;
            aimAction.performed += OnAimPerformed;

            DisablePlayerInput();
        }

        private void OnDisable()
        {
            shootAction.performed -= OnShootPerformed;
            aimAction.performed -= OnAimPerformed;

            DisablePlayerInput();
        }

        public void EnablePlayerInput()
        {
            combatMap.Enable();
        }

        public void DisablePlayerInput()
        {
            combatMap.Disable();
        }

        private void OnShootPerformed(InputAction.CallbackContext ctx)
        {
            TriggerShoot();
        }

        private void OnAimPerformed(InputAction.CallbackContext ctx)
        {
            if (!CombatInputRules.CanAim) return;

            Vector2 value = ctx.ReadValue<Vector2>();
            OnAim?.Invoke(value);
        }

        private void TriggerShoot()
        {
            if (!CombatInputRules.CanShoot) return;

            OnShoot?.Invoke();
        }
    }
}