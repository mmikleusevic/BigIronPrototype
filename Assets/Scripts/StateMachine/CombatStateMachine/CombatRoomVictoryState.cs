using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomVictoryState : IState
    {
        private readonly CombatRoomController combatRoomController;
        
        public CombatRoomVictoryState(CombatRoomController controller)
        {
            combatRoomController = controller;
        }
        
        public UniTask OnEnter()
        {
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