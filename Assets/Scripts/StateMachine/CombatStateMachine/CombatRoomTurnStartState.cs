using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Enemies;
using Managers;
using PokerDiceRoom;
using StateMachine.PokerStateMachine;
using UnityEngine;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomTurnStartState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly CombatRoomEvents combatRoomEvents;
        
        public CombatRoomTurnStartState(CombatRoomController controller)
        {
            combatRoomController = controller;
            combatRoomEvents = combatRoomController.CombatRoomEvents;
        }        
        
        public async UniTask OnEnter(CancellationToken externalToken)
        {
            if (combatRoomController.TurnQueue.Count == 0)
            {
                combatRoomController.CalculateTurnOrder();
            }

            combatRoomController.GetNextAliveCombatant();
            
            if (!combatRoomController.CurrentCombatant)
            {
                await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomCheckWinState(combatRoomController));
                return;
            }
            
            Combatant current = combatRoomController.CurrentCombatant;
            combatRoomEvents.OnTurnStarted(current.Data.combatantName);
            
            SoundManager.Instance.PlayVFX(combatRoomController.TurnSound);
    
            if (current.Data.combatantType == CombatantType.Player)
            {
                await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomPlayerTurnState(combatRoomController));
            }
            else
            {
                await combatRoomController.BaseStateMachine.ChangeState(new CombatRoomEnemyTurnState(combatRoomController));
            }
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