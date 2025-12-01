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
        private readonly CombatTargetInputs combatTargetInputs;
        private readonly CombatRoomEvents combatRoomEvents;
        
        private List<EnemyCombatant> enemies;
        private int enemyIndex;

        public CombatRoomPlayerTargetSelectingState(CombatRoomManager manager)
        {
            combatRoomManager = manager;
            combatTargetInputs = combatRoomManager.CombatTargetInputs;
            combatRoomEvents = combatRoomManager.CombatRoomEvents;
        }
        
        public UniTask OnEnter()
        {
            combatRoomEvents.OnPlayerTargetSelectingStarted?.Invoke();
            
            enemies = combatRoomManager.GetAliveEnemies();
            enemyIndex = 0;

            HighlightEnemy(enemies[enemyIndex]);
            
            combatTargetInputs.OnMove += Move;
            combatTargetInputs.OnConfirm += Confirm;
            combatTargetInputs.OnCancel += Cancel;
            
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            
        }

        public UniTask OnExit()
        {
            combatRoomEvents.OnPlayerTargetSelectingEnded?.Invoke();
            
            combatTargetInputs.DisablePlayerTurnInput();
            combatTargetInputs.OnMove -= Move;
            combatTargetInputs.OnConfirm -= Confirm;
            combatTargetInputs.OnCancel -= Cancel;
            
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

        private void Cancel()
        {
            combatRoomManager.BaseStateMachine.ChangeState(new CombatRoomPlayerTurnState(combatRoomManager)).Forget();
        }
        
        private void HighlightEnemy(EnemyCombatant enemyCombatant) => enemyCombatant?.EnemyUI?.Show();
        private void UnhighlightEnemy(EnemyCombatant enemyCombatant) => enemyCombatant?.EnemyUI?.Hide();
    }
}