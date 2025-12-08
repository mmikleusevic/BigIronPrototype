using System.Collections.Generic;
using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers;
using UnityEngine;
using CameraController = CombatRoom.CameraController;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomPlayerTargetSelectingState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly CombatTargetInputs combatTargetInputs;
        private readonly CombatRoomEvents combatRoomEvents;
        
        private List<EnemyCombatant> enemies;
        private int enemyIndex;

        public CombatRoomPlayerTargetSelectingState(CombatRoomController controller)
        {
            combatRoomController = controller;
            combatTargetInputs = combatRoomController.CombatTargetInputs;
            combatRoomEvents = combatRoomController.CombatRoomEvents;
        }
        
        public UniTask OnEnter(CancellationToken externalToken)
        {
            combatRoomEvents?.OnPlayerTargetSelectingStarted?.Invoke();
            
            enemies = combatRoomController.GetAliveEnemies();
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
            combatRoomEvents?.OnPlayerTargetSelectingEnded?.Invoke();
            
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
            combatRoomController.HandleTargetChosen(enemies[enemyIndex]);
            combatRoomController.BaseStateMachine.ChangeState(new CombatRoomPlayerAttackState(combatRoomController)).Forget();
        }

        private void Cancel()
        {
            combatRoomController.BaseStateMachine.ChangeState(new CombatRoomPlayerTurnState(combatRoomController)).Forget();
        }
        
        private void HighlightEnemy(EnemyCombatant enemyCombatant) => enemyCombatant?.EnemyUI?.Show();
        private void UnhighlightEnemy(EnemyCombatant enemyCombatant) => enemyCombatant?.EnemyUI?.Hide();
    }
}