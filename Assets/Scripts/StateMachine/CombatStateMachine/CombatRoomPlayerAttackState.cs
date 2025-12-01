using CombatRoom;
using Cysharp.Threading.Tasks;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerAttackState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        private readonly CombatRoomEvents combatRoomEvents;
        
        public CombatRoomPlayerAttackState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
            combatRoomEvents = combatRoomManager.CombatRoomEvents;
        }
        
        public UniTask OnEnter()
        {
            combatRoomEvents.OnPlayerAttackStarted?.Invoke();
            
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            combatRoomEvents.OnPlayerAttackEnded?.Invoke();
            
            return UniTask.CompletedTask;
        }
    }
}