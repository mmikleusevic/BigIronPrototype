using System;
using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Enemies;
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
        private readonly EnemyCombatant currentEnemy;
        
        private const int InitialCountdownTime = 5;
        
        private Gun gun;
        private bool countdownFinished;
        
        public CombatRoomPlayerAttackState(CombatRoomController controller)
        {
            combatRoomController = controller;
            combatRoomEvents = combatRoomController.CombatRoomEvents;
            combatInputs = combatRoomController.CombatInputs;
            cameraController = combatRoomController.CameraController;
            if (GameManager.Instance) playerCombatant = GameManager.Instance.PlayerCombatant;
            currentEnemy = combatRoomController.SelectedEnemy;
        }
        
        public async UniTask OnEnter(CancellationToken externalToken)
        {
            SetupAttackPhase();
                
            playerCombatant.StartAiming();
            
            await StartCountdown(InitialCountdownTime, externalToken);
                
            EnablePlayerControls();
                
            await StartAttackCountdown(currentEnemy.AttackDuration, externalToken);
            await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomPlayerDamageResultState(combatRoomController));
        }

        public void OnUpdate()
        {
            if (!countdownFinished) return;
            
            var rawLookInput = combatInputs.AimValue;
            if (rawLookInput == Vector2.zero) return;

            float sensitivity = CameraManager.Instance?.GetAimSensitivity() ?? 1f;
            
            var finalLookInput = combatInputs.IsAimingWithController
                ? rawLookInput * (sensitivity * Time.deltaTime * 100f)
                : rawLookInput * sensitivity;
                
            playerCombatant.HandleLook(finalLookInput);
        }

        public UniTask OnExit()
        {
            CleanupAttackPhase();
            
            return UniTask.CompletedTask;
        }
        
        private void SetupAttackPhase()
        {
            combatRoomEvents?.OnPlayerAttackStarted?.Invoke();
            combatRoomController?.ToggleEnemyVisibility(false);
            
            countdownFinished = false;
            gun = playerCombatant.Gun;
            
            Cursor.lockState = CursorLockMode.Locked;
            cameraController.SwitchToPlayerCamera();
        }
        
        private void CleanupAttackPhase()
        {
            combatRoomEvents?.OnPlayerAttackEnded?.Invoke();
            combatRoomController?.SelectedEnemy?.StopSpawningTargets();
            combatRoomController?.ToggleEnemyVisibility(true);
            
            cameraController.SwitchToOverviewCamera();
            Cursor.lockState = CursorLockMode.None;
            
            combatInputs.OnShoot -= Shoot;
            combatInputs.OnReload -= Reload;
            combatInputs.DisablePlayerInput();

            if (playerCombatant) playerCombatant.ResetAim();
        }
        
        private async UniTask StartCountdown(int seconds, CancellationToken token)
        {
            for (int i = seconds; i > 0; i--)
            {
                combatRoomEvents?.OnCountdownTick?.Invoke(i);
                await UniTask.Delay(1000, cancellationToken: token);
            }

            combatRoomEvents?.OnCountdownTick?.Invoke(0);
            countdownFinished = true;
        }
        
        private async UniTask StartAttackCountdown(float attackDuration, CancellationToken token)
        {
            int secondsRemaining = Mathf.CeilToInt(attackDuration);

            while (secondsRemaining > 0 && !token.IsCancellationRequested)
            {
                combatRoomEvents?.OnAttackCountdownTick(secondsRemaining);
                await UniTask.Delay(1000, cancellationToken: token);
                secondsRemaining--;
            }

            combatRoomEvents?.OnAttackCountdownTick(0);
        }

        private void EnablePlayerControls()
        {
            combatRoomController?.SelectedEnemy?.SpawnTargets();
            
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