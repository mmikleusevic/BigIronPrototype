using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers;
using Player;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerDamageResultState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly CombatRoomEvents combatRoomEvents;
        
        private EnemyCombatant targetEnemy;
        private float damageMultiplier;
        
        private const float ResultDisplayDuration = 2f;

        public CombatRoomPlayerDamageResultState(CombatRoomController controller)
        {
            combatRoomController = controller;
            combatRoomEvents = combatRoomController.CombatRoomEvents;
        }
        
        public async UniTask OnEnter(CancellationToken externalToken)
        {
            targetEnemy = combatRoomController.SelectedEnemy;
            targetEnemy?.EnemyUI?.Show();
            
            await UniTask.Delay(1000, cancellationToken: externalToken);
            
            combatRoomEvents?.OnPlayerDamageResultStarted?.Invoke();
            
            await UniTask.WaitUntil(() => damageMultiplier >= 0f || externalToken.IsCancellationRequested, 
                cancellationToken: externalToken);
            
            
            await UniTask.Delay((int)(ResultDisplayDuration * 1000), cancellationToken: externalToken);
            
            targetEnemy?.EnemyUI?.Hide();
            
            await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomTurnEndState(combatRoomController));
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            combatRoomEvents?.OnPlayerDamageResultEnded?.Invoke();
            
            targetEnemy?.EnemyUI?.Hide();
            
            return UniTask.CompletedTask;
        }
    }
}