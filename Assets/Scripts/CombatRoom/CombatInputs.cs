using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatRoom
{
    public class CombatInputs : MonoBehaviour, ICombatInputSource
    {
        public event Action OnShoot;
        public event Action OnReload;

        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private CombatRoomController combatRoomController;
        
        private InputActionMap combatMap;
        private InputAction shootAction;
        private InputAction aimAction;
        private InputAction reloadAction;

        private CombatInputRules CombatInputRules => combatRoomController.CombatInputRules;

        public bool IsAimingWithController => aimAction.activeControl?.device is Gamepad;
        public Vector2 AimValue => aimAction.ReadValue<Vector2>();

        private void OnEnable()
        {
            combatMap = inputActionAsset.FindActionMap(GameStrings.COMBAT);

            shootAction = combatMap.FindAction(GameStrings.SHOOT);
            aimAction = combatMap.FindAction(GameStrings.AIM);
            reloadAction = combatMap.FindAction(GameStrings.RELOAD);

            shootAction.performed += OnShootPerformed;
            reloadAction.performed += OnReloadPerformed;

            DisablePlayerInput();
        }

        private void OnDisable()
        {
            shootAction.performed -= OnShootPerformed;
            reloadAction.performed -= OnReloadPerformed;

            DisablePlayerInput();
        }

        public void EnablePlayerInput()
        {
            combatMap.Enable();
        }
        
        public void EnableAiming()
        {
            aimAction.Enable();
        }

        public void DisablePlayerInput()
        {
            combatMap.Disable();
        }

        private void OnShootPerformed(InputAction.CallbackContext ctx)
        {
            TriggerShoot();
        }
        
        private void OnReloadPerformed(InputAction.CallbackContext ctx)
        {
            TriggerReload();
        }

        private void TriggerReload()
        {
            if (!CombatInputRules.CanReload) return;
            
            OnReload?.Invoke();
        }

        private void TriggerShoot()
        {
            if (!CombatInputRules.CanShoot) return;

            OnShoot?.Invoke();
        }
    }
}