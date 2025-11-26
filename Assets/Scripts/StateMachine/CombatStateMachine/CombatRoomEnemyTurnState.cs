using CombatRoom;
using Cysharp.Threading.Tasks;
using Player;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomEnemyTurnState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        
        public CombatRoomEnemyTurnState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
        }

        public async UniTask OnEnter()
        {
            await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomTurnEndState(combatRoomManager));
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