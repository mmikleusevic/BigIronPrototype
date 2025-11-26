using CombatRoom;
using Cysharp.Threading.Tasks;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomSetupState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        
        public CombatRoomSetupState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
        }
        
        public async UniTask OnEnter()
        {
            combatRoomManager.SpawnEnemies();
            
            await UniTask.Delay(1000);
            
            combatRoomManager.CalculateTurnOrder();
            
            await combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomTurnStartState(combatRoomManager));
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