using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomCheckWinState : IState
    {
        private readonly CombatRoomController combatRoomController;

        public CombatRoomCheckWinState(CombatRoomController controller)
        {
            combatRoomController = controller;
        }

        public async UniTask OnEnter()
        {
            if (combatRoomController.CheckWinCondition())
            {
                await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomVictoryState(combatRoomController));
            }
            else if (combatRoomController.CheckLossCondition())
            {
                await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomDefeatState(combatRoomController));
            }
            else
            {
                await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomTurnStartState(combatRoomController));
            }
        }

        public void OnUpdate() { }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}