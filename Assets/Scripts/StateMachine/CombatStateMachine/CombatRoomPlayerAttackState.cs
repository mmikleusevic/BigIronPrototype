using CombatRoom;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
            
            combatInputs.EnablePlayerInput();
            combatInputs.OnShoot += Shoot;
            combatInputs.OnAim += Aim;
            
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
            combatInputs.DisablePlayerInput();
            
            return UniTask.CompletedTask;
        }

        private void Shoot()
        {
            
        }

        private void Aim(Vector2 move)
        {

        }
    }
}