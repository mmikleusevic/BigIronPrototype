using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomDefeatState : IState
    {
        private readonly CombatRoomEvents combatRoomEvents;
        
        public CombatRoomDefeatState(CombatRoomController controller)
        {
            combatRoomEvents = controller.CombatRoomEvents;
        }
        
        public UniTask OnEnter()
        {
            combatRoomEvents.OnDefeatStarted?.Invoke();
            
            GameManager.Instance.GameOver(false);
            
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