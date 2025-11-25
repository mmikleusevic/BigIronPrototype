using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace PokerDiceRoom
{
    public class PokerInputs : MonoBehaviour, IPokerInputSource
    {
        public event Action OnRoll;
        public event Action OnHold;
        public event Action<Vector2> OnMove;
        public event Action OnSelect;
        public event Action OnEnd;
        
        [Header("Input Action References")]
        [SerializeField] private InputActionReference moveSelection;
        [SerializeField] private InputActionReference select;
        [SerializeField] private InputActionReference roll;
        [SerializeField] private InputActionReference holdTurn;
        [SerializeField] private InputActionReference end;
        [SerializeField] private PokerDiceGameManager gameManager;
        private PokerInputRules Rules => gameManager.PokerInputRules;
        
        private InputActionReference[] actions;
        private bool InputEnabled => !gameManager.PokerGame.CurrentPlayer.IsAI || gameManager.IsGameOver;
    
        private void Awake()
        {
            actions = new[] { moveSelection, select, roll, holdTurn, end };
        }

        private void OnEnable()
        {
            if (moveSelection) moveSelection.action.performed += OnMovePerformed;
            if (select) select.action.performed += DiceSelected;
            if (roll) roll.action.performed += OnRollPerformed;
            if (holdTurn) holdTurn.action.performed += OnHoldPerformed;
            if (end) end.action.performed += OnEndPerformed;
            
            EnableAll(true);
        }

        private void OnDisable()
        {
            if (moveSelection) moveSelection.action.performed -= OnMovePerformed;
            if (select) select.action.performed -= DiceSelected;
            if (roll) roll.action.performed -= OnRollPerformed;
            if (holdTurn) holdTurn.action.performed -= OnHoldPerformed;
            if (end) end.action.performed -= OnEndPerformed;
            
            EnableAll(false);
        }
    
        private void EnableAll(bool enable)
        {
            foreach (InputActionReference inputActionReference in actions)
            {
                if (!inputActionReference) continue;
                
                if (enable)
                {
                    inputActionReference.action.Enable();
                }
                else
                {
                    inputActionReference.action.Disable();
                }
            }
        }
        
        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            if (!InputEnabled || !Rules.CanMove) return;
            
            Vector2 value = ctx.ReadValue<Vector2>();
            OnMove?.Invoke(value);
        }

        private void DiceSelected(InputAction.CallbackContext ctx)
        {
            TriggerSelect();
        }

        private void OnRollPerformed(InputAction.CallbackContext ctx)
        {
            TriggerRoll();
        }

        private void OnHoldPerformed(InputAction.CallbackContext ctx)
        {
            TriggerHold();
        }
        
        private void OnEndPerformed(InputAction.CallbackContext obj)
        {
            TriggerEnd();
        }
        
        public void TriggerRoll()
        {
            if (!InputEnabled || !Rules.CanRoll) return;
            
            OnRoll?.Invoke();
        }

        public void TriggerHold()
        {
            if (!InputEnabled || !Rules.CanHold) return;
            
            OnHold?.Invoke();
        }

        private void TriggerSelect()
        {
            if (!InputEnabled || !Rules.CanSelect) return;
            
            OnSelect?.Invoke();
        }

        private void TriggerEnd()
        {
            if (!InputEnabled || !Rules.CanEnd) return;
            
            OnEnd?.Invoke();
        }
    }
}