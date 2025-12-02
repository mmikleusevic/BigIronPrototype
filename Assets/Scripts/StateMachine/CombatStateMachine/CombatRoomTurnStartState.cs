using CombatRoom;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers;
using PokerDiceRoom;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomTurnStartState : IState
    {
        private readonly CombatRoomController combatRoomController;
        
        public CombatRoomTurnStartState(CombatRoomController controller)
        {
            combatRoomController = controller;
        }        
        
        public async UniTask OnEnter()
        {
            if (combatRoomController.TurnQueue.Count == 0)
            {
                combatRoomController.CalculateTurnOrder();
            }

            combatRoomController.GetNextAliveCombatant();
            
            if (!combatRoomController.CurrentCombatant)
            {
                await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomCheckWinState(combatRoomController));
                return;
            }
            
            Combatant current = combatRoomController.CurrentCombatant;
    
            if (current.Type == CombatantType.Player)
            {
                await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomPlayerTurnState(combatRoomController));
            }
            else
            {
                await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomEnemyTurnState(combatRoomController));
            }
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