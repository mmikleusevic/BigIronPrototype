using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers;
using Player;
using UnityEngine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerDamageResultState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly PlayerComboSystem playerComboSystem;
        private readonly PlayerCombatant playerCombatant;
        
        private EnemyCombatant targetEnemy;
        
        private const float ResultDisplayDuration = 2f;

        public CombatRoomPlayerDamageResultState(CombatRoomController controller)
        {
            combatRoomController = controller;
            playerComboSystem = combatRoomController.PlayerComboSystem;
            if (GameManager.Instance) playerCombatant = GameManager.Instance.PlayerCombatant;
        }
        
        public async UniTask OnEnter(CancellationToken externalToken)
        {
            targetEnemy = combatRoomController.SelectedEnemy;
            targetEnemy?.EnemyUI?.Show();
            
            await UniTask.Delay(1000, cancellationToken: externalToken);
            
            using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken,
                playerCombatant.GetCancellationTokenOnDestroy()
            );
            
            int totalDamage = (int)(playerCombatant.Data.damage * playerComboSystem.DamageMultiplier);
            if (totalDamage > 0)
            {
                await playerCombatant.RotateTowardsTargetAndFire(combatRoomController.SelectedEnemy, totalDamage, linkedCts.Token);
                await UniTask.Delay((int)(ResultDisplayDuration * 1000), cancellationToken: externalToken);
            }
            
            targetEnemy?.EnemyUI?.Hide();
            
            await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomTurnEndState(combatRoomController));
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            targetEnemy?.EnemyUI?.Hide();
            
            return UniTask.CompletedTask;
        }
    }
}