using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using EventRoom;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceRollingState : IState
    {
        private readonly PokerDiceGameController pokerDiceGameController;
        private readonly PokerInputs pokerInputSource;
        private readonly PokerInputRules pokerInputRules;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        private readonly PokerGameEvents pokerGameEvents;
        
        private int selectedDieIndex;
        private bool hasDoneAction;
        
        public PokerDiceRollingState(PokerDiceGameController controller)
        {
            pokerDiceGameController = controller;
            pokerInputSource = pokerDiceGameController.PokerInputs;
            pokerInputRules = pokerDiceGameController.PokerInputRules;
            diceRoller = pokerDiceGameController.DiceRoller;
            pokerGame = pokerDiceGameController.PokerGame;
            pokerGameEvents = pokerDiceGameController.PokerGameEvents;
        }
        
        public UniTask OnEnter(CancellationToken externalToken)
        {
            SubscribeInputs();
         
            PokerPlayer player = pokerGame.CurrentPlayer;
            diceRoller.SetPlayerRolls(player);
            int playerRolls = diceRoller.GetNumberOfPlayerRolls(player);
            
            pokerGameEvents?.OnDiceRollingStarted?.Invoke(playerRolls, diceRoller.MaxRolls);
            
            ToggleHighlight();
            
            pokerInputSource.EnablePlayerTurnInput();

            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {

        }
        
        public UniTask OnExit()
        {
            UnsubscribeInputs();
            
            pokerGameEvents?.OnDiceRollingEnded?.Invoke();

            return UniTask.CompletedTask;
        }
        
        private void SubscribeInputs()
        {
            pokerInputSource.OnRoll += Roll;
            pokerInputSource.OnHold += HoldTurn;
            pokerInputSource.OnMove += MoveSelection;
            pokerInputSource.OnSelect += Select;
        }

        private void UnsubscribeInputs()
        {
            pokerInputSource.OnRoll -= Roll;
            pokerInputSource.OnHold -= HoldTurn;
            pokerInputSource.OnMove -= MoveSelection;
            pokerInputSource.OnSelect -= Select;
        }
        
        private void Roll()
        {
            if (hasDoneAction) return;

            hasDoneAction = true;
            
            pokerGameEvents?.OnRoll?.Invoke();
            
            ToggleHighlight();
            pokerInputSource.DisablePlayerTurnInput();

            if (!diceRoller) return;
            
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
            
            pokerGameEvents?.OnHold?.Invoke();
            pokerInputSource.DisablePlayerTurnInput();
            
            ToggleHighlight();
            ChangeState();
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
        
        private void RollComplete() => ChangeState();
        
        private void ChangeState()
        {
            pokerDiceGameController.BaseStateMachine.ChangeState(new PokerDiceTurnEndState(pokerDiceGameController)).Forget();
        }
        
        private Die CurrentDie => diceRoller.PlayerDice[pokerGame.CurrentPlayer][selectedDieIndex];

        private void ToggleHighlight()
        {
            if (pokerInputRules.CanMove) CurrentDie?.DieVisual.ToggleHighlight();
        }
    }
}