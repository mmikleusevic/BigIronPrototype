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

        [Header("Input Action References")] [SerializeField]
        private InputActionAsset inputActionAsset;
        [SerializeField] private PokerDiceGameController gameController;
        private PokerInputRules Rules => gameController.PokerInputRules;
        
        private InputActionMap pokerDiceMap;
        private InputAction moveAction;
        private InputAction selectAction;
        private InputAction rollAction;
        private InputAction holdAction;
        private InputAction endAction;
        private bool InputEnabled => !gameController.PokerGame.CurrentPlayer.IsAI || gameController.IsGameOver;

        private void OnEnable()
        {
            pokerDiceMap = inputActionAsset.FindActionMap(GameStrings.POKER_GAME);
            
            moveAction = pokerDiceMap.FindAction(GameStrings.MOVE);
            selectAction = pokerDiceMap.FindAction(GameStrings.SELECT);
            rollAction = pokerDiceMap.FindAction(GameStrings.ROLL);
            holdAction = pokerDiceMap.FindAction(GameStrings.HOLD);
            endAction = pokerDiceMap.FindAction(GameStrings.END);
            
            moveAction.performed += OnMovePerformed;
            selectAction.performed += DiceSelected;
            rollAction.performed += OnRollPerformed;
            holdAction.performed += OnHoldPerformed;
            endAction.performed += OnEndPerformed;
            
            Debug.Log(pokerDiceMap.enabled);
            
            DisablePlayerTurnInput();
        }

        private void OnDisable()
        {
            moveAction.performed -= OnMovePerformed;
            selectAction.performed -= DiceSelected;
            rollAction.performed -= OnRollPerformed;
            holdAction.performed -= OnHoldPerformed;
            endAction.performed -= OnEndPerformed;
            
            DisablePlayerTurnInput();
        }
        
        public void EnablePlayerTurnInput()
        {
            pokerDiceMap.Enable();
            
            Debug.Log(pokerDiceMap.enabled);
        }
        
        public void DisablePlayerTurnInput()
        {
            pokerDiceMap.Disable();
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