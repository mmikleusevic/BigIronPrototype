using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceAIRollingState : IState
    {
        private readonly PokerDiceGameController pokerDiceGameController;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        private int selectedDieIndex;
        
        public PokerDiceAIRollingState(PokerDiceGameController controller)
        {
            pokerDiceGameController = controller;
            diceRoller = pokerDiceGameController.DiceRoller;
            pokerGame = pokerDiceGameController.PokerGame;
        }
        
        public async UniTask OnEnter(CancellationToken externalToken)
        {
            PokerPlayer player = pokerGame.CurrentPlayer;
            diceRoller.SetPlayerRolls(player);

            await StartAIRollRoutine(externalToken);
        }
        
        private async UniTask StartAIRollRoutine(CancellationToken externalToken)
        {
            await UniTask.Delay(1000, cancellationToken: externalToken);
            
            if (diceRoller.CurrentRollNumber > 0) await SelectSmartDiceToRoll(externalToken);
            
            OnRoll();
            
            await UniTask.Delay(1500, cancellationToken: externalToken);
        }
        
        public void OnUpdate()
        {

        }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
        
        private async UniTask SelectSmartDiceToRoll(CancellationToken externalToken)
        {
            PokerPlayer player = pokerGame.CurrentPlayer;

            PokerDiceHandResult opponentHand = pokerGame.GetOpponentBestHand();

            List<(int Index, int Value)> dice = diceRoller.PlayerDice[player]
                .Select((die, index) => (Index: index, Value: die.Value))
                .ToList();

            PokerDiceHandResult myHand = PokerDiceHandEvaluation.EvaluateHand(player, dice.Select(d => d.Value).ToList());

            HashSet<int> keep = PokerDiceAIHelper.SelectDiceToKeep(dice, myHand, opponentHand);

            List<int> toRoll = dice
                .Where(d => !keep.Contains(d.Index))
                .Select(d => d.Index)
                .ToList();

            if (toRoll.Count == 0) return;

            int maxIndex = toRoll.Max();

            for (int i = selectedDieIndex; i <= maxIndex; i++)
            {
                selectedDieIndex = i;
                ToggleHighlight();

                if (toRoll.Contains(i))
                {
                    CurrentDie.ToggleDie();
                }

                await UniTask.Delay(500, cancellationToken: externalToken);
                ToggleHighlight();
            }
        }
        
        private void OnRoll()
        {
            if (!diceRoller) return;
            
            diceRoller.RollDice(pokerGame.CurrentPlayer,rolls => 
            {
                pokerGame.SetPlayerRolls(rolls);
                OnRollComplete();
            });
        }
        
        private void OnRollComplete() => ChangeState();
        
        private void ChangeState()
        {
            pokerDiceGameController.BaseStateMachine.ChangeState(new PokerDiceTurnEndState(pokerDiceGameController)).Forget();
        }
        
        private Die CurrentDie => diceRoller.PlayerDice[pokerGame.CurrentPlayer][selectedDieIndex];
        
        private void ToggleHighlight()
        {
            CurrentDie?.DieVisual?.ToggleHighlight();
        }
    }
}