using System.Threading;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Managers;

namespace StateMachine.CombatStateMachine
{
    public class CombatRoomVictoryState : IState
    {
        private readonly CombatRoomController combatRoomController;
        private readonly CombatRoomEvents combatRoomEvents;
        private readonly CombatInputs combatInputs;
        
        public CombatRoomVictoryState(CombatRoomController controller)
        {
            combatRoomController = controller;
            combatRoomEvents = combatRoomController.CombatRoomEvents;
            combatInputs = combatRoomController.CombatInputs;
        }
        
        public UniTask OnEnter(CancellationToken externalToken)
        {
            combatRoomEvents.OnVictoryStarted?.Invoke();
            
            combatInputs.OnEnd += EndWrapper;
            combatInputs.EnableEnd();
            
            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            
        }
        
        private void EndWrapper()
        {
            End().Forget();
        }
        
        private async UniTask End()
        {
            combatInputs.OnEnd -= EndWrapper;
            combatInputs.DisablePlayerInput();

            if (!LevelManager.Instance) return;
            
            await LevelManager.Instance.UnloadSceneActivateGame(combatRoomController.CombatRoomAssetReference);
        }

        public UniTask OnExit()
        {
            if (InputManager.Instance) InputManager.Instance.EnableOnlyUIMap();
            if (GameManager.Instance) GameManager.Instance.RoomPassed();
            
            return UniTask.CompletedTask;
        }
    }
}