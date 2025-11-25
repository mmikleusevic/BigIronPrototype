using CombatRoom;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomSetupState : IPokerDiceState
    {
        private readonly CombatRoomManager combatRoomManager;
        
        
        public CombatRoomSetupState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
        }
        
        public void OnEnter()
        {
            combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomTurnStartState(combatRoomManager));
        }

        public void OnUpdate()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}