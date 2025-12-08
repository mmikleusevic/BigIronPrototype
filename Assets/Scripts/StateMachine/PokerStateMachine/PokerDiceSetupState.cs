using System.Threading;
using Cysharp.Threading.Tasks;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceSetupState : IState
    {
        private readonly PokerDiceGameController pokerDiceGameController;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        public PokerDiceSetupState(PokerDiceGameController controller)
        {
            pokerDiceGameController = controller;
            diceRoller = pokerDiceGameController.DiceRoller;
            pokerGame = pokerDiceGameController.PokerGame;
        }

        public async UniTask OnEnter(CancellationToken externalToken)
        {
            diceRoller.CurrentRollNumber = 0;
            
            await pokerGame.Initialize();
            await diceRoller.Initialize(pokerGame.Players);
            await pokerDiceGameController.BaseStateMachine.ChangeState(new PokerDiceTurnStartState(pokerDiceGameController));
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