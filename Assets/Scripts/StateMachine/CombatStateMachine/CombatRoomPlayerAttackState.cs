using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.Rendering;
using Weapons;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerAttackState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly CombatRoomEvents combatRoomEvents;
        private readonly CombatInputs combatInputs;
        private readonly PlayerCombatant playerCombatant;

        private Gun gun;
        private bool countdownFinished;
        
        private const int InitialCountdownTime = 5;
        private const int AttackDurationSeconds = 15;
        
        public CombatRoomPlayerAttackState(CombatRoomController controller)
        {
            combatRoomController = controller;
            combatRoomEvents = combatRoomController.CombatRoomEvents;
            combatInputs = combatRoomController.CombatInputs;
            playerCombatant = combatRoomController.CurrentCombatant as PlayerCombatant;
        }
        
        public async UniTask OnEnter()
        {
            combatRoomEvents.OnPlayerAttackStarted?.Invoke();
            
            countdownFinished = false;

            gun = playerCombatant.Gun;
            
            await StartCountdown(InitialCountdownTime);
            
            EnablePlayerControls();
            
            await UniTask.Delay(AttackDurationSeconds * 1000);
            await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomTurnEndState(combatRoomController));
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            combatRoomEvents.OnPlayerAttackEnded?.Invoke();
            
            combatInputs.OnShoot -= Shoot;
            combatInputs.OnAim -= Aim;
            combatInputs.OnReload -= Reload;
            combatInputs.DisablePlayerInput();
            
            return UniTask.CompletedTask;
        }
        
        private async UniTask StartCountdown(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                combatRoomEvents.OnCountdownTick?.Invoke(i);
                await UniTask.Delay(1000);
            }

            combatRoomEvents.OnCountdownTick?.Invoke(0);
            countdownFinished = true;
        }

        private void EnablePlayerControls()
        {
            combatRoomEvents.OnPlayerCanAttack?.Invoke();
            
            combatInputs.OnShoot += Shoot;
            combatInputs.OnAim += Aim;
            combatInputs.OnReload += Reload;
            combatInputs.EnablePlayerInput();
        }

        private void Shoot()
        {
            if (!countdownFinished) return;
            
            gun?.Shoot();
        }

        private void Aim(Vector2 move)
        {
            if (!countdownFinished) return;
        }

        private void Reload()
        {
            if (!countdownFinished) return;
            
            gun?.Reload();
        }
    }
}