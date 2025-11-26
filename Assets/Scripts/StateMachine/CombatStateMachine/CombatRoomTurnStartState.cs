using CombatRoom;
using Cysharp.Threading.Tasks;
using Enemies;
using PokerDiceRoom;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomTurnStartState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        
        public CombatRoomTurnStartState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
        }        
        
        public async UniTask OnEnter()
        {
            if (combatRoomManager.TurnQueue.Count == 0)
            {
                combatRoomManager.CalculateTurnOrder();
            }

            combatRoomManager.GetNextAliveCombatant();
            
            if (!combatRoomManager.CurrentCombatant)
            {

                await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomCheckWinState(combatRoomManager));
                return;
            }
            
            Combatant current = combatRoomManager.CurrentCombatant;
    
            if (current.Type == CombatantType.Player)
            {
                await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomPlayerTurnState(combatRoomManager));
            }
            else
            {
                await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomEnemyTurnState(combatRoomManager));
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