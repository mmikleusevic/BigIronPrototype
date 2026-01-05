using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomSetupState : IState
    {
        private readonly CombatRoomController combatRoomController;
        
        public CombatRoomSetupState(CombatRoomController controller)
        {
            combatRoomController = controller;
        }
        
        public async UniTask OnEnter(CancellationToken externalToken)
        {
            combatRoomController.SpawnEnemies();
            
            await UniTask.Delay(1000, cancellationToken: externalToken);
            
            combatRoomController.CalculateTurnOrder();
            
            await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomTurnStartState(combatRoomController));
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