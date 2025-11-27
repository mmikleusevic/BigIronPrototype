using System.Collections.Generic;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Enemies;
using UnityEngine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerTargetSelectingState : IState
    {
        private readonly CombatRoomManager combatRoomManager;
        private readonly CombatTargetInputs combatTargetInputSource;
        
        private List<EnemyCombatant> enemies;
        private int enemyIndex;

        public CombatRoomPlayerTargetSelectingState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
            combatTargetInputSource = combatRoomManager.CombatTargetInputs;
        }
        
        public UniTask OnEnter()
        {
            enemies = combatRoomManager.GetAliveEnemies();
            enemyIndex = 0;

            HighlightEnemy(enemies[enemyIndex]);
        
            combatTargetInputSource.EnablePlayerTurnInput();
            combatTargetInputSource.OnMove += Move;
            combatTargetInputSource.OnConfirm += Confirm;
            
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            combatTargetInputSource.DisablePlayerTurnInput();
            combatTargetInputSource.OnMove -= Move;
            combatTargetInputSource.OnConfirm -= Confirm;

            UnhighlightEnemy(enemies[enemyIndex]);
            
            return UniTask.CompletedTask;
        }
        
        private void Move(Vector2 move)
        {
            UnhighlightEnemy(enemies[enemyIndex]);
            
            if (move.x > 0.5f) enemyIndex++;
            else if (move.x < -0.5f) enemyIndex--;

            enemyIndex = (enemyIndex + enemies.Count) % enemies.Count;

            HighlightEnemy(enemies[enemyIndex]);
        }
        
        private void Confirm()
        {
            combatRoomManager.HandleTargetChosen(enemies[enemyIndex]);
            combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomPlayerAttackState(combatRoomManager)).Forget();
        }
        
        private void HighlightEnemy(EnemyCombatant enemyCombatant) => enemyCombatant?.EnemyUI?.Show();
        private void UnhighlightEnemy(EnemyCombatant enemyCombatant) => enemyCombatant?.EnemyUI?.Hide();
    }
}