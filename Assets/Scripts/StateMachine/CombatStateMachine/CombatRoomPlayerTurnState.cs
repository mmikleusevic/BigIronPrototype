using CombatRoom;
using Cysharp.Threading.Tasks;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerTurnState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        private readonly CombatRoomEvents combatRoomEvents;
        private readonly CombatTargetInputs combatTargetInputs;
        
        public CombatRoomPlayerTurnState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
            combatRoomEvents = combatRoomManager.CombatRoomEvents;
            combatTargetInputs = combatRoomManager.CombatTargetInputs;
        }        
        
        public UniTask OnEnter()
        {
            combatRoomEvents.OnPlayerTurnStarted?.Invoke();

            combatTargetInputs.EnablePlayerTurnInput();
            combatTargetInputs.OnShootSelected += ShootSelected;
            
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            combatRoomEvents.OnPlayerTurnEnded?.Invoke();
            
            combatTargetInputs.OnShootSelected -= ShootSelected;
            
            return UniTask.CompletedTask;
        }

        private void ShootSelected()
        {
            combatRoomManager.BaseStateMachine
                .ChangeState(new CombatRoomPlayerTargetSelectingState(combatRoomManager)).Forget();
        }
    }
}