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
        [SerializeField] private TextMeshProUGUI wagerText;
        [SerializeField] private TextMeshProUGUI numberOfRoles;
        [SerializeField] private Button rollButton;
        [SerializeField] private Button holdButton;
        [SerializeField] private Button endButton;
        [SerializeField] private AssetReference gameAssetReference;
        [SerializeField] private AssetReference pokerAssetReference;
        [SerializeField] private PokerDiceGameManager pokerDiceGameManager;
        private PokerInputs PokerInputs => pokerDiceGameManager.PokerInputs;
        private PokerInputRules InputRules => pokerDiceGameManager.PokerInputRules;
        private PokerGame PokerGame => pokerDiceGameManager.PokerGame;
        
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
            DiceRoller.OnNumberOfRollsChanged += SetNumberOfRolls;
            PokerDiceTurnStartState.OnTurnStart += SetCurrentPlayerText;
            PokerDiceRollingState.OnDiceRollingStarted += OnDiceRollStarted;
            PokerDiceRollingState.OnRoll += DisableGameButtons;
            PokerDiceRollingState.OnHold += DisableGameButtons;
            PokerDiceEvaluatingState.OnDiceEvaluationStarted += OnDiceEvaluationStarted;
            PokerDiceTurnEndState.OnTurnEndStarted += OnTurnEnded;
            PokerDiceGameOverState.OnGameOver += OnGameOver;
            PokerGame.OnWagerChanged += SetWagerText;
            InputRules.OnRulesChanged += OnRulesChanged;
            rollButton.onClick.AddListener(OnRollPressed);
            holdButton.onClick.AddListener(OnHoldPressed);
            endButton.AddClickAsync(OnEndPressed);
        }

        private void OnDisable()
        {
            DiceRoller.OnNumberOfRollsChanged -= SetNumberOfRolls;
            PokerDiceTurnStartState.OnTurnStart -= SetCurrentPlayerText;
            PokerDiceRollingState.OnDiceRollingStarted -= OnDiceRollStarted;
            PokerDiceRollingState.OnRoll -= DisableGameButtons;
            PokerDiceRollingState.OnHold -= DisableGameButtons;
            PokerDiceEvaluatingState.OnDiceEvaluationStarted -= OnDiceEvaluationStarted;
            PokerDiceTurnEndState.OnTurnEndStarted -= OnTurnEnded;
            PokerDiceGameOverState.OnGameOver -= OnGameOver;
            PokerGame.OnWagerChanged -= SetWagerText;
            InputRules.OnRulesChanged -= OnRulesChanged;
            rollButton.onClick.RemoveListener(OnRollPressed);
            holdButton.onClick.RemoveListener(OnHoldPressed);
            endButton.onClick.RemoveAllListeners();
        }

        private void SetWagerText(int wager)
        {
            wagerText.text = $"Wager: {wager}";
        }
        
        private void OnDiceRollStarted(int numberOfRolls, int maxNumberOfRolls)
        {
            if (numberOfRolls <= 1)
            {
                rollButton.gameObject.SetActive(true);
                return;
            }

            holdButton.gameObject.SetActive(true);
            rollButton.gameObject.SetActive(true);
        }

        private void SetCurrentPlayerText(PokerPlayer player)
        {
            playerNameText.gameObject.SetActive(true);
            playerNameText.text = "Currently playing: " + player.PlayerName;
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
        
        private void OnGameOver()
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

        private void DisableGameButtons()
        {
            rollButton.gameObject.SetActive(false);
            holdButton.gameObject.SetActive(false);
        }
        
        private async UniTask OnEndPressed()
        {
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