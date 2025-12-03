using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerTurnState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly CombatRoomEvents combatRoomEvents;
        private readonly CombatTargetInputs combatTargetInputs;
        
        public CombatRoomPlayerTurnState(CombatRoomController controller)
        {
            combatRoomController = controller;
            combatRoomEvents = combatRoomController.CombatRoomEvents;
            combatTargetInputs = combatRoomController.CombatTargetInputs;
        }        
        
        public UniTask OnEnter()
        {
            combatRoomEvents?.OnPlayerTurnStarted?.Invoke();

            combatTargetInputs.EnablePlayerTurnInput();
            combatTargetInputs.OnShootSelected += ShootSelected;
            
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            combatRoomEvents?.OnPlayerTurnEnded?.Invoke();
            
            combatTargetInputs.OnShootSelected -= ShootSelected;
            
            return UniTask.CompletedTask;
        }

        private void ShootSelected()
        {
            combatRoomController.BaseStateMachine
                .ChangeState(new CombatRoomPlayerTargetSelectingState(combatRoomController)).Forget();
        }
    }
}