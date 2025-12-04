using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomDefeatState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly CombatRoomEvents combatRoomEvents;
        
        public CombatRoomDefeatState(CombatRoomController controller)
        {
            combatRoomController = controller;
            combatRoomEvents = combatRoomController.CombatRoomEvents;
        }
        
        public UniTask OnEnter()
        {
            combatRoomEvents.OnDefeatStarted?.Invoke();
            
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}