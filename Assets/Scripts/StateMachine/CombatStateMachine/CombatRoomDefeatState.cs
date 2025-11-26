using CombatRoom;
using Cysharp.Threading.Tasks;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomDefeatState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        
        public CombatRoomDefeatState(CombatRoomManager manager)
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