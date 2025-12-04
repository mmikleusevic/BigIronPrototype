using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.Rendering;
using Weapons;
using CameraController = CombatRoom.CameraController;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerAttackState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly CombatRoomEvents combatRoomEvents;
        private readonly CombatInputs combatInputs;
        private readonly CameraController cameraController;
        private readonly PlayerCombatant playerCombatant;
        
        private const int InitialCountdownTime = 5;
        private const int AttackDurationSeconds = 15;
        
        private Gun gun;
        
        private bool countdownFinished;
        
        public CombatRoomPlayerAttackState(CombatRoomController controller)
        {
            combatRoomController = controller;
            combatRoomEvents = combatRoomController.CombatRoomEvents;
            combatInputs = combatRoomController.CombatInputs;
            cameraController = combatRoomController.CameraController;
            if (GameManager.Instance) playerCombatant = GameManager.Instance.PlayerCombatant;
        }
        
        public async UniTask OnEnter()
        {
            combatRoomEvents?.OnPlayerAttackStarted?.Invoke();

            countdownFinished = false;
            gun = playerCombatant.Gun;
            
            Cursor.lockState = CursorLockMode.Locked;
            cameraController.SwitchToPlayerCamera();
            
            combatInputs.EnableAiming();
            
            await StartCountdown(InitialCountdownTime);
            
            EnablePlayerControls();
            
            await UniTask.Delay(AttackDurationSeconds * 1000);
            await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomTurnEndState(combatRoomController));
        }

        public void OnUpdate()
        {
            Vector2 rawLookInput = combatInputs.AimValue;
            Vector2 finalLookInput = Vector2.zero;
            
            Debug.Log(rawLookInput);

            if (rawLookInput == Vector2.zero) return;

            float sensitivity = 0;

            if (CameraManager.Instance) sensitivity = CameraManager.Instance.GetAimSensitivity();
                
            if (combatInputs.IsAimingWithController)
            {
                finalLookInput = rawLookInput * (sensitivity * Time.deltaTime * 100f);
            }
            else
            {
                finalLookInput = rawLookInput * sensitivity;
            }
                
            playerCombatant.HandleLook(finalLookInput);
        }

        public UniTask OnExit()
        {
            combatRoomEvents?.OnPlayerAttackEnded?.Invoke();
            
            cameraController.SwitchToOverviewCamera();
            
            Cursor.lockState = CursorLockMode.None;
            
            combatInputs.OnShoot -= Shoot;
            combatInputs.OnReload -= Reload;
            combatInputs.DisablePlayerInput();
            
            playerCombatant.ResetAim();
            
            return UniTask.CompletedTask;
        }
        
        private async UniTask StartCountdown(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                combatRoomEvents?.OnCountdownTick?.Invoke(i);
                await UniTask.Delay(1000);
            }

            combatRoomEvents?.OnCountdownTick?.Invoke(0);
            countdownFinished = true;
        }

        private void EnablePlayerControls()
        {
            combatRoomEvents?.OnPlayerCanAttack?.Invoke();
            
            combatInputs.OnShoot += Shoot;
            combatInputs.OnReload += Reload;
            combatInputs.EnablePlayerInput();
        }

        private void Shoot()
        {
            if (!countdownFinished) return;
            
            playerCombatant.ExecuteShoot();
        }

        private void Reload()
        {
            if (!countdownFinished) return;
            
            gun?.Reload();
        }
    }
}