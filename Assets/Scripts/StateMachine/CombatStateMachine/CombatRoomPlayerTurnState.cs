using CombatRoom;
using Cysharp.Threading.Tasks;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerTurnState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        
        // This is just to delay currently so that it doesn't go to infinite loop
        private bool delay = true;
        
        public CombatRoomPlayerTurnState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
        }        
        
        public async UniTask OnEnter()
        {
            //TODO remove this condition later
            if (!delay) await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomTurnEndState(combatRoomManager));
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