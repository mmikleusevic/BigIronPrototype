using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers;
using Player;
using StateMachine.PokerStateMachine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomEnemyTurnState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly EnemyCombatant currentEnemy;
        private readonly PlayerCombatant playerCombatant;
        
        public CombatRoomEnemyTurnState(CombatRoomController controller)
        {
            combatRoomController = controller;
            currentEnemy = combatRoomController.CurrentCombatant as EnemyCombatant;
            if (GameManager.Instance) playerCombatant = GameManager.Instance.PlayerCombatant;
        }

        public async UniTask OnEnter(CancellationToken externalToken)
        {
            await UniTask.Delay(1000, cancellationToken: externalToken);
            
            //TODO maybe try to await the attack back
            currentEnemy.Attack(playerCombatant);
            
            await UniTask.Delay(1000, cancellationToken: externalToken);
            
            await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomTurnEndState(combatRoomController));
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