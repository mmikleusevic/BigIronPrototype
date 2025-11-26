using CombatRoom;
using Cysharp.Threading.Tasks;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomTurnEndState : IState
    {
        private readonly CombatRoomManager combatRoomManager;

        public CombatRoomTurnEndState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
        }

        public async UniTask OnEnter()
        {
            await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomCheckWinState(combatRoomManager));
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