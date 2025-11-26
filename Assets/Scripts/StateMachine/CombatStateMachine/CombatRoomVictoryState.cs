using CombatRoom;
using Cysharp.Threading.Tasks;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomVictoryState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        
        public CombatRoomVictoryState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
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