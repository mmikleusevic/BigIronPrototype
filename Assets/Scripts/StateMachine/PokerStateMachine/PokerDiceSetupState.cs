using Cysharp.Threading.Tasks;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceSetupState : IState
    {
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        public PokerDiceSetupState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            diceRoller = pokerDiceGameManager.DiceRoller;
            pokerGame = pokerDiceGameManager.PokerGame;
        }

        public async UniTask OnEnter()
        {
            diceRoller.CurrentRollNumber = 0;
            
            await pokerGame.Initialize();
            await diceRoller.Initialize(pokerGame.Players);
            await pokerDiceGameManager.BaseStateMachine.ChangeState(new PokerDiceTurnStartState(pokerDiceGameManager));
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