using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions;
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
        [SerializeField] private Button endButton;
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
            endButton.gameObject.SetActive(false);
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
            endButton.AddClickAsync(OnEndPressed);
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
            endButton.onClick.RemoveAllListeners();
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
            endButton.gameObject.SetActive(true);
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
        
        private async UniTask OnEndPressed()
        {
            //TODO test this more
            _ = LevelManager.Instance.UnloadSceneAsync(pokerAssetReference.AssetGUID);
            await LevelManager.Instance.LoadSceneAsync(gameAssetReference);
        }
        
        private void OnRulesChanged()
        {
            rollButton.gameObject.SetActive(InputRules.CanRoll);
            holdButton.gameObject.SetActive(InputRules.CanHold);
            endButton.gameObject.SetActive(InputRules.CanEnd);
        }
    }
}