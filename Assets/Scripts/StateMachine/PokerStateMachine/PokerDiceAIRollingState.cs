using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceAIRollingState : IPokerDiceState
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
        
        public void OnEnter()
        {
            PokerPlayer player = pokerGame.CurrentPlayer;
            diceRoller.SetPlayerRolls(player);

            if (diceRoller.CurrentRollNumber > 0) ToggleHighlight();
            
            _ = StartAIRollRoutine();
        }
        
        private async UniTaskVoid StartAIRollRoutine()
        {
            Debug.Log($"{pokerGame.CurrentPlayer} (AI) is thinking...");

            await UniTask.Delay(1000);
            
            Debug.Log(diceRoller.CurrentRollNumber);
            
            if (diceRoller.CurrentRollNumber > 0) await SelectAllDiceToRoll();
            
            OnRoll();
            
            await UniTask.Delay(1500);
        }
        
        private async UniTask SelectAllDiceToRoll()
        {
            for (int i = 0; i < diceRoller.PlayerDice[pokerGame.CurrentPlayer].Count; i++)
            {
                ToggleHighlight();
                
                selectedDieIndex = i;
                
                Die die = diceRoller.PlayerDice[pokerGame.CurrentPlayer][i];
                
                ToggleHighlight();
                die.ToggleDie();

                await UniTask.Delay(300);
            }

            await UniTask.CompletedTask;
        }
        
        private void OnRoll()
        {
            if (diceRoller.CurrentRollNumber > 0) ToggleHighlight();
            
            diceRoller.RollDice(pokerGame.CurrentPlayer,rolls => 
            {
                pokerGame.SetPlayerRolls(rolls);
                OnRollComplete();
            });
        }
        
        private void OnRollComplete()
        {
            Debug.Log($"Dice: {string.Join(", ", diceRoller.PlayerDice.Where(a => a.Key == pokerGame.CurrentPlayer).SelectMany(d => d.Value.Select(c => c.Value)))}");
            EndTurn();
        }
        
        private void EndTurn()
        {
            pokerDiceGameManager.StateMachine.ChangeState(new PokerDiceTurnEndState(pokerDiceGameManager));
        }
        
        private Die CurrentDie => diceRoller.PlayerDice[pokerGame.CurrentPlayer][selectedDieIndex];
        
        private void ToggleHighlight()
        {
            CurrentDie?.DieVisual.ToggleHighlight();
        }
        
        public void OnUpdate()
        {

        }

        public void OnExit()
        {

        }
    }
}