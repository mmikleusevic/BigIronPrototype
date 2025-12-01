using CombatRoom;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerAttackState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        private readonly CombatRoomEvents combatRoomEvents;
        private readonly CombatInputs combatInputs;
        
        public CombatRoomPlayerAttackState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
            combatRoomEvents = combatRoomManager.CombatRoomEvents;
            combatInputs = combatRoomManager.CombatInputs;
        }
        
        public UniTask OnEnter()
        {
            combatRoomEvents.OnPlayerAttackStarted?.Invoke();
            
            combatInputs.OnShoot += Shoot;
            combatInputs.OnAim += Aim;
            combatInputs.OnReload += Reload;
            combatInputs.EnablePlayerInput();
            
            return UniTask.CompletedTask;
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

        private void Shoot()
        {
            
        }

        private void Aim(Vector2 move)
        {

        }

        private void Reload()
        {
            
        }
    }
}