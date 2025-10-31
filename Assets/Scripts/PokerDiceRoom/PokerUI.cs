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
    public class PokerUI : MonoBehaviour, IPokerInputSource
    {
        public event Action OnRoll;
        public event Action OnHold;
        public event Action<Vector2> OnMove;
        public event Action OnSelect;

        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI numberOfRoles;
        [SerializeField] private TextMeshProUGUI handResult;
        [SerializeField] private Button rollButton;
        [SerializeField] private Button holdButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private AssetReference gameAssetReference;
        [SerializeField] private AssetReference pokerAssetReference;
        
        private void Awake()
        {
            playerNameText.gameObject.SetActive(false);
            numberOfRoles.gameObject.SetActive(false);
            handResult.gameObject.SetActive(false);
            rollButton.gameObject.SetActive(false);
            holdButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            PokerDiceTurnStartState.OnTurnStart += SetCurrentPlayerText;
            PokerDiceRollingState.OnDiceRollingStarted += OnDiceRollStarted;
            PokerDiceEvaluatingState.OnHandEvaluated += SetHandResult;
            PokerDiceTurnEndState.OnTurnEndStarted += OnTurnEnded;
            PokerDiceGameOverState.OnGameOver += OnGameOver;
            rollButton.onClick.AddListener(OnRollPressed);
            holdButton.onClick.AddListener(OnHoldPressed);
            continueButton.onClick.AddListener(OnContinuePressed);
        }

        private void OnDisable()
        {
            PokerDiceTurnStartState.OnTurnStart -= SetCurrentPlayerText;
            PokerDiceRollingState.OnDiceRollingStarted -= OnDiceRollStarted;
            PokerDiceEvaluatingState.OnHandEvaluated -= SetHandResult;
            PokerDiceTurnEndState.OnTurnEndStarted -= OnTurnEnded;
            PokerDiceGameOverState.OnGameOver -= OnGameOver;
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
        
        private void SetHandResult(PokerDiceHandResult pokerDiceHandResult)
        {
            handResult.gameObject.SetActive(true);
            handResult.text +=
                $"{pokerDiceHandResult.PlayerName} " +
                $"got: {pokerDiceHandResult.Description} " +
                $"(Score: {pokerDiceHandResult.Score})\n";
        }
        
        private void OnGameOver(string playerName)
        {
            playerNameText.gameObject.SetActive(false);
            numberOfRoles.gameObject.SetActive(false);
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
            OnRoll?.Invoke();
        }
        
        private void OnHoldPressed()
        {
            rollButton.gameObject.SetActive(false);
            holdButton.gameObject.SetActive(false);
            OnHold?.Invoke();
        }
        
        private void OnContinuePressed()
        {
            _ = LevelManager.Instance.LoadSceneAsync(gameAssetReference);
            _ = LevelManager.Instance.UnloadSceneAsync(pokerAssetReference.AssetGUID);
        }
    }
}