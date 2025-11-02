using System;
using System.Collections.Generic;
using Managers;
using StateMachine.PokerStateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace PokerDiceRoom
{
    public class PokerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI numberOfRoles;
        [SerializeField] private Button rollButton;
        [SerializeField] private Button holdButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private AssetReference gameAssetReference;
        [SerializeField] private AssetReference pokerAssetReference;
        [SerializeField] private PokerDiceGameManager pokerDiceGameManager;
        private PokerInputs PokerInputs => pokerDiceGameManager.PokerInputs;
        private PokerInputRules InputRules => pokerDiceGameManager.PokerInputRules;
        
        private void Awake()
        {
            playerNameText.gameObject.SetActive(false);
            numberOfRoles.gameObject.SetActive(false);
            rollButton.gameObject.SetActive(false);
            holdButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            PokerDiceTurnStartState.OnTurnStart += SetCurrentPlayerText;
            PokerDiceRollingState.OnDiceRollingStarted += OnDiceRollStarted;
            PokerDiceEvaluatingState.OnDiceEvaluationStarted += OnDiceEvaluationStarted;
            PokerDiceTurnEndState.OnTurnEndStarted += OnTurnEnded;
            PokerDiceGameOverState.OnGameOver += OnGameOver;
            InputRules.OnRulesChanged += OnRulesChanged;
            rollButton.onClick.AddListener(OnRollPressed);
            holdButton.onClick.AddListener(OnHoldPressed);
            continueButton.onClick.AddListener(OnContinuePressed);
        }

        private void OnDisable()
        {
            PokerDiceTurnStartState.OnTurnStart -= SetCurrentPlayerText;
            PokerDiceRollingState.OnDiceRollingStarted -= OnDiceRollStarted;
            PokerDiceEvaluatingState.OnDiceEvaluationStarted -= OnDiceEvaluationStarted;
            PokerDiceTurnEndState.OnTurnEndStarted -= OnTurnEnded;
            PokerDiceGameOverState.OnGameOver -= OnGameOver;
            InputRules.OnRulesChanged -= OnRulesChanged;
            rollButton.onClick.RemoveListener(OnRollPressed);
            holdButton.onClick.RemoveListener(OnHoldPressed);
            continueButton.onClick.RemoveListener(OnContinuePressed);
        }

        private void OnDiceRollStarted(int numberOfRolls, int maxNumberOfRolls)
        {
            SetNumberOfRolls(numberOfRolls, maxNumberOfRolls);
            
            if (numberOfRolls <= 1)
            {
                rollButton.gameObject.SetActive(true);
                return;
            }

            holdButton.gameObject.SetActive(true);
            rollButton.gameObject.SetActive(true);
        }

        private void SetCurrentPlayerText(string playerName)
        {
            playerNameText.gameObject.SetActive(true);
            playerNameText.text = "Current player: " + playerName;
        }

        private void SetNumberOfRolls(int currentNumberOfRolls, int maxNumberOfRolls)
        {
            numberOfRoles.gameObject.SetActive(true);
            numberOfRoles.text = "Rolls: " + currentNumberOfRolls + "/" + maxNumberOfRolls;
        }

        private void OnDiceEvaluationStarted()
        {
            playerNameText.gameObject.SetActive(false);
            numberOfRoles.gameObject.SetActive(false);
        }
        
        private void OnGameOver(string playerName)
        {
            continueButton.gameObject.SetActive(true);
        }
        
        private void OnTurnEnded()
        {
            rollButton.gameObject.SetActive(false);
            holdButton.gameObject.SetActive(false);
        }
        
        private void OnRollPressed()
        {
            rollButton.gameObject.SetActive(false);
            holdButton.gameObject.SetActive(false);

            PokerInputs.TriggerRoll();
        }
        
        private void OnHoldPressed()
        {
            rollButton.gameObject.SetActive(false);
            holdButton.gameObject.SetActive(false);
            
            PokerInputs.TriggerHold();
        }
        
        private void OnContinuePressed()
        {
            _ = LevelManager.Instance.LoadSceneAsync(gameAssetReference);
            _ = LevelManager.Instance.UnloadSceneAsync(pokerAssetReference.AssetGUID);
        }
        
        private void OnRulesChanged()
        {
            rollButton.gameObject.SetActive(InputRules.CanRoll);
            holdButton.gameObject.SetActive(InputRules.CanHold);
            continueButton.gameObject.SetActive(InputRules.CanContinue);
        }
    }
}