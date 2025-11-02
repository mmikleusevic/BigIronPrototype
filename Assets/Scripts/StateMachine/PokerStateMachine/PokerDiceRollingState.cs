using System;
using System.Collections.Generic;
using System.Linq;
using EventRoom;
using PokerDiceRoom;
using UnityEngine;

namespace StateMachine.PokerStateMachine
{
    public class PokerDiceRollingState : IPokerDiceState
    {
        public static event Action<int, int> OnDiceRollingStarted;
        public static event Action OnDiceRollingEnded;
        
        private readonly PokerDiceGameManager pokerDiceGameManager;
        private readonly IPokerInputSource pokerInputSource;
        private readonly DiceRoller diceRoller;
        private readonly PokerGame pokerGame;
        
        private int selectedDieIndex;
        
        public PokerDiceRollingState(PokerDiceGameManager manager)
        {
            pokerDiceGameManager = manager;
            pokerInputSource = pokerDiceGameManager.PokerInputs;
            diceRoller = pokerDiceGameManager.DiceRoller;
            pokerGame = pokerDiceGameManager.PokerGame;
        }
        
        public void OnEnter()
        {
            pokerInputSource.OnRoll += OnRoll;
            pokerInputSource.OnHold += OnHoldTurn;
            pokerInputSource.OnMove += OnMoveSelection;
            pokerInputSource.OnSelect += OnSelect;
            
            string pokerPlayer = pokerGame.CurrentPlayer;
            diceRoller.SetPlayerRolls(pokerPlayer);
            int playerRolls = diceRoller.GetPlayerRolls(pokerPlayer);
            
            diceRoller.PlayerDice[pokerGame.CurrentPlayer][selectedDieIndex].DieVisual.ToggleHighlight();
            
            OnDiceRollingStarted?.Invoke(playerRolls, diceRoller.MaxRolls);
        }

        public void OnUpdate() { }

        private void OnRoll()
        {
            diceRoller.PlayerDice[pokerGame.CurrentPlayer][selectedDieIndex].DieVisual.ToggleHighlight();
            
            diceRoller.RollDice(pokerGame.CurrentPlayer,rolls => 
            {
                pokerGame.SetPlayerRolls(rolls);
                OnRollComplete();
            });
        }

        private void OnHoldTurn()
        {
            DetermineNextState();
        }
        
        private void OnMoveSelection(Vector2 move)
        {
            diceRoller.PlayerDice[pokerGame.CurrentPlayer][selectedDieIndex].DieVisual.ToggleHighlight();
            
            if (move.x > 0.5f) selectedDieIndex++;
            else if (move.x < -0.5f) selectedDieIndex--;
            
            int diceCount = diceRoller.PlayerDice[pokerGame.CurrentPlayer].Count;
            selectedDieIndex = (selectedDieIndex + diceCount) % diceCount;
            
            diceRoller.PlayerDice[pokerGame.CurrentPlayer][selectedDieIndex].DieVisual.ToggleHighlight();
        }
        
        private void OnSelect()
        { 
            diceRoller.PlayerDice[pokerGame.CurrentPlayer][selectedDieIndex].ToggleDie();
        }
        
        private void OnRollComplete()
        {
            Debug.Log($"Dice: {string.Join(", ", diceRoller.PlayerDice.Where(a => a.Key == pokerGame.CurrentPlayer).SelectMany(d => d.Value.Select(c => c.Value)))}");
            DetermineNextState();
        }

        private void DetermineNextState()
        {
            if (diceRoller.AllPlayersRolled() && diceRoller.CurrentRollNumber >= diceRoller.MaxRolls)
            {
                pokerDiceGameManager.StateMachine.ChangeState(new PokerDiceEvaluatingState(pokerDiceGameManager));
            }
            else
            {
                pokerDiceGameManager.StateMachine.ChangeState(new PokerDiceTurnEndState(pokerDiceGameManager));
            }
        }
        
        public void OnExit()
        {
            OnDiceRollingEnded?.Invoke();
            
            pokerInputSource.OnRoll -= OnRoll;
            pokerInputSource.OnHold -= OnHoldTurn;
            pokerInputSource.OnMove -= OnMoveSelection;
            pokerInputSource.OnSelect -= OnSelect;
        }
    }
}