using CombatRoom;
using Cysharp.Threading.Tasks;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerTurnState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        private readonly CombatRoomEvents combatRoomEvents;
        
        public CombatRoomPlayerTurnState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
            combatRoomEvents = combatRoomManager.CombatRoomEvents;
        }        
        
        public UniTask OnEnter()
        {
            combatRoomEvents.OnPlayerTurnStarted?.Invoke();

            combatRoomManager.OnShoot += HandleShootChosen;
            
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            combatRoomEvents.OnPlayerTurnEnded?.Invoke();
            
            combatRoomManager.OnShoot -= HandleShootChosen;
            
            return UniTask.CompletedTask;
        }

        private void HandleShootChosen()
        {
            combatRoomManager.BaseStateMachine
                .ChangeState(new CombatRoomPlayerTargetSelectingState(combatRoomManager)).Forget();
        }
    }
}