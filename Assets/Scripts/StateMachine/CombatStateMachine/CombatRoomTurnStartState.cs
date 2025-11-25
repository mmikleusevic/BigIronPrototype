using CombatRoom;
using PokerDiceRoom;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomTurnStartState : IPokerDiceState
    {
        private readonly CombatRoomManager combatRoomManager;
        
        public CombatRoomTurnStartState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
        }        
        
        public void OnEnter()
        {
            
        }

        public void OnUpdate()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}