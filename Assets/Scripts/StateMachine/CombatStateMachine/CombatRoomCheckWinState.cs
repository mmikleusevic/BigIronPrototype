using CombatRoom;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomCheckWinState : IState
    {
        private readonly CombatRoomManager combatRoomManager;

        public CombatRoomCheckWinState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
        }

        public async UniTask OnEnter()
        {
            if (combatRoomManager.CheckWinCondition())
            {
                await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomVictoryState(combatRoomManager));
            }
            else if (combatRoomManager.CheckLossCondition())
            {
                await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomDefeatState(combatRoomManager));
            }
            else
            {
                await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomTurnStartState(combatRoomManager));
            }
        }

        public void OnUpdate() { }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
    }
}