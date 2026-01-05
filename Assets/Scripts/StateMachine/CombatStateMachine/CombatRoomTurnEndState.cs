using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomTurnEndState : IState
    {
        private readonly CombatRoomController combatRoomController;

        public CombatRoomTurnEndState(CombatRoomController controller)
        {
            combatRoomController = controller;
        }

        public async UniTask OnEnter(CancellationToken externalToken)
        {
            await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomCheckWinState(combatRoomController));
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