using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceSetupState : IPokerDiceState
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

        public void OnEnter()
        {
            diceRoller.CurrentRollNumber = 0;
            
            pokerGame.Initialize();
            diceRoller.Initialize(pokerGame.Players);

            pokerDiceGameManager.StateMachine.ChangeState(new PokerDiceTurnStartState(pokerDiceGameManager));
        }

        public void OnUpdate() { }

        public void OnExit() { }
    }
}