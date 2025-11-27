using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceAIRollingState : IState
    {
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        private int selectedDieIndex;
        
        public PokerDiceAIRollingState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            diceRoller = pokerDiceGameManager.DiceRoller;
            pokerGame = pokerDiceGameManager.PokerGame;
        }
        
        public async UniTask OnEnter()
        {
            PokerPlayer player = pokerGame.CurrentPlayer;
            diceRoller.SetPlayerRolls(player);

            await StartAIRollRoutine();
        }
        
        private async UniTask StartAIRollRoutine()
        {
            Debug.Log($"{pokerGame.CurrentPlayer} (AI) is thinking...");

            await UniTask.Delay(1000);
            
            Debug.Log(diceRoller.CurrentRollNumber);
            
            if (diceRoller.CurrentRollNumber > 0) await SelectSmartDiceToRoll();
            
            OnRoll();
            
            await UniTask.Delay(1500);
        }
        
        public void OnUpdate()
        {

        }

        public UniTask OnExit()
        {
            return UniTask.CompletedTask;
        }
        
        private async UniTask SelectSmartDiceToRoll()
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

                await UniTask.Delay(500);
                ToggleHighlight();
            }
        }
        
        private void OnRoll()
        {
            diceRoller?.RollDice(pokerGame.CurrentPlayer,rolls => 
            {
                pokerGame.SetPlayerRolls(rolls);
                OnRollComplete();
            });
        }
        
        private void OnRollComplete()
        {
            Debug.Log($"Dice: {string.Join(", ", diceRoller.PlayerDice.Where(a => a.Key == pokerGame.CurrentPlayer).SelectMany(d => d.Value.Select(c => c.Value)))}");
            ChangeState();
        }
        
        private void ChangeState()
        {
            pokerDiceGameManager.BaseStateMachine.ChangeState(new PokerDiceTurnEndState(pokerDiceGameManager)).Forget();
        }
        
        private Die CurrentDie => diceRoller.PlayerDice[pokerGame.CurrentPlayer][selectedDieIndex];
        
        private void ToggleHighlight()
        {
            CurrentDie?.DieVisual.ToggleHighlight();
        }
    }
}