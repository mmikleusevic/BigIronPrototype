using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using EventRoom;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceRollingState : IPokerDiceState
    {
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly IPokerInputSource pokerInputSource;
        private readonly PokerInputRules pokerInputRules;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        private readonly PokerGameEvents pokerGameEvents;
        
        private int selectedDieIndex;
        private bool hasDoneAction;
        
        public PokerDiceRollingState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            pokerInputSource = pokerDiceGameManager.PokerInputs;
            pokerInputRules = pokerDiceGameManager.PokerInputRules;
            diceRoller = pokerDiceGameManager.DiceRoller;
            pokerGame = pokerDiceGameManager.PokerGame;
            pokerGameEvents = pokerDiceGameManager.PokerGameEvents;
        }
        
        public void OnEnter()
        {
            pokerInputSource.OnRoll += Roll;
            pokerInputSource.OnHold += HoldTurn;
            pokerInputSource.OnMove += MoveSelection;
            pokerInputSource.OnSelect += Select;
         
            PokerPlayer player = pokerGame.CurrentPlayer;
            diceRoller.SetPlayerRolls(player);
            int playerRolls = diceRoller.GetNumberOfPlayerRolls(player);
            
            pokerGameEvents.OnDiceRollingStarted?.Invoke(playerRolls, diceRoller.MaxRolls);
            
            ToggleHighlight();
        }

        public void OnUpdate() { }

        private void Roll()
        {
            if (hasDoneAction) return;

            hasDoneAction = true;
            
            pokerGameEvents.OnRoll?.Invoke();
            
            ToggleHighlight();
            
            diceRoller.RollDice(pokerGame.CurrentPlayer,rolls => 
            {
                pokerGame.SetPlayerRolls(rolls);
                RollComplete();
            });
        }

        private void HoldTurn()
        {
            if (hasDoneAction) return;

            hasDoneAction = true;
            
            pokerGameEvents.OnHold?.Invoke();
            
            ToggleHighlight();
            EndTurn();
        }
        
        private void MoveSelection(Vector2 move)
        {
            if (hasDoneAction) return;
            
            ToggleHighlight();
            
            if (move.x > 0.5f) selectedDieIndex++;
            else if (move.x < -0.5f) selectedDieIndex--;
            
            int diceCount = diceRoller.ReturnNumberOfDice(pokerGame.CurrentPlayer);
            selectedDieIndex = (selectedDieIndex + diceCount) % diceCount;
            
            ToggleHighlight();
        }
        
        private void Select()
        {
            if (hasDoneAction) return;
            
            CurrentDie?.ToggleDie();
        }
        
        private void RollComplete()
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
            if (pokerInputRules.CanMove) CurrentDie?.DieVisual.ToggleHighlight();
        }
        
        public void OnExit()
        {
            pokerGameEvents.OnDiceRollingEnded?.Invoke();
            
            pokerInputSource.OnRoll -= Roll;
            pokerInputSource.OnHold -= HoldTurn;
            pokerInputSource.OnMove -= MoveSelection;
            pokerInputSource.OnSelect -= Select;
        }
    }
}