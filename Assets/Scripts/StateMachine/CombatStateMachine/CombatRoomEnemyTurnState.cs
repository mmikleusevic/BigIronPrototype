using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;
using Player;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomEnemyTurnState : IState
    {
        private readonly CombatRoomController combatRoomController;
        
        public CombatRoomEnemyTurnState(CombatRoomController controller)
        {
            combatRoomController = controller;
        }

        public async UniTask OnEnter()
        {
            await UniTask.Delay(1000);
            
            await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomTurnEndState(combatRoomController));
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