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
        public event Action OnEnd;

        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private CombatRoomController combatRoomController;
        
        private InputActionMap combatMap;
        private InputAction shootAction;
        private InputAction aimAction;
        private InputAction reloadAction;
        private InputAction endAction;

        private CombatInputRules CombatInputRules => combatRoomController.CombatInputRules;

        public bool IsAimingWithController => aimAction.activeControl?.device is Gamepad;
        public Vector2 AimValue => CombatInputRules.CanAim ? aimAction.ReadValue<Vector2>() : Vector2.zero;

        private void OnEnable()
        {
            combatMap = inputActionAsset.FindActionMap(GameStrings.COMBAT);

            shootAction = combatMap.FindAction(GameStrings.SHOOT);
            aimAction = combatMap.FindAction(GameStrings.AIM);
            reloadAction = combatMap.FindAction(GameStrings.RELOAD);
            endAction = combatMap.FindAction(GameStrings.END);
            
            shootAction.performed += OnShootPerformed;
            reloadAction.performed += OnReloadPerformed;
            endAction.performed += OnEndPerformed;

            DisablePlayerInput();
        }

        private void OnDisable()
        {
            shootAction.performed -= OnShootPerformed;
            reloadAction.performed -= OnReloadPerformed;
            endAction.performed -= OnEndPerformed;

            DisablePlayerInput();
        }

        public void EnablePlayerInput()
        {
            combatMap.Enable();
        }

        public void EnableEnd()
        {
            endAction.Enable();
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
        
        private void OnEndPerformed(InputAction.CallbackContext ctx)
        {
            TriggerEnd();
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

        private void TriggerEnd()
        {
            if (!CombatInputRules.CanEnd) return;

            OnEnd?.Invoke();
        }
    }
}