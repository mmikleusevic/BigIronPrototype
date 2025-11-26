using System;
using Cysharp.Threading.Tasks;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceTurnStartState : IState
    {
        private readonly PokerGameEvents pokerGameEvents;
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        private readonly float delayDuration = 1f;
    
        public PokerDiceTurnStartState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            pokerGameEvents = pokerDiceGameManager.PokerGameEvents;
            diceRoller = pokerDiceGameManager.DiceRoller;
            pokerGame = pokerDiceGameManager.PokerGame;
        }
    
        public async UniTask OnEnter()
        {
            Debug.Log($"=== {pokerGame.CurrentPlayer}'s Turn ===");
            
            diceRoller.ResetDiceHolds(pokerGame.CurrentPlayer);
            pokerGameEvents.OnTurnStart?.Invoke(pokerGame.CurrentPlayer);
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayDuration));
        
            if (pokerGame.CurrentPlayer.IsAI)
            {
                await pokerDiceGameManager.BaseStateMachine.ChangeState(new PokerDiceAIRollingState(pokerDiceGameManager));    
            }
            else
            {
                await pokerDiceGameManager.BaseStateMachine.ChangeState(new PokerDiceRollingState(pokerDiceGameManager));
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