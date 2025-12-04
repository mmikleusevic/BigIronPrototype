using Cysharp.Threading.Tasks;
using Extensions;
using Managers;
using PokerDiceRoom;
using StateMachine.PokerStateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class PokerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI wagerText;
        [SerializeField] private TextMeshProUGUI numberOfRoles;
        [SerializeField] private Button rollButton;
        [SerializeField] private Button holdButton;
        [SerializeField] private Button endButton;
        [SerializeField] private PokerDiceGameController pokerDiceGameController;
        
        private PokerInputs PokerInputs => pokerDiceGameController.PokerInputs;
        private PokerInputRules InputRules => pokerDiceGameController.PokerInputRules;
        private PokerGame PokerGame => pokerDiceGameController.PokerGame;
        private PokerGameEvents PokerGameEvents => pokerDiceGameController.PokerGameEvents;
        private DiceRoller DiceRoller => pokerDiceGameController.DiceRoller;
        
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
            PokerGameEvents.OnTurnStart += SetCurrentPlayerText;
            PokerGameEvents.OnDiceRollingStarted += OnDiceRollStarted;
            PokerGameEvents.OnRoll += DisableGameButtons;
            PokerGameEvents.OnHold += DisableGameButtons;
            PokerGameEvents.OnDiceEvaluationStarted += OnDiceEvaluationStarted;
            PokerGameEvents.OnTurnEndStarted += OnTurnEnded;
            PokerGameEvents.OnGameOver += OnGameOver;
            PokerGame.OnWagerChanged += SetWagerText;
            InputRules.OnRulesChanged += OnRulesChanged;
            rollButton.onClick.AddListener(OnRollPressed);
            holdButton.onClick.AddListener(OnHoldPressed);
            endButton.AddClickAsync(OnEndPressed);
        }

        private void OnDisable()
        {
            DiceRoller.OnNumberOfRollsChanged -= SetNumberOfRolls;
            PokerGameEvents.OnTurnStart -= SetCurrentPlayerText;
            PokerGameEvents.OnDiceRollingStarted -= OnDiceRollStarted;
            PokerGameEvents.OnRoll -= DisableGameButtons;
            PokerGameEvents.OnHold -= DisableGameButtons;
            PokerGameEvents.OnDiceEvaluationStarted -= OnDiceEvaluationStarted;
            PokerGameEvents.OnTurnEndStarted -= OnTurnEnded;
            PokerGameEvents.OnGameOver -= OnGameOver;
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
            if (EventSystem.current) EventSystem.current.SetSelectedGameObject(null);
            
            PokerInputs.TriggerRoll();
        }
        
        private void OnHoldPressed()
        {
            rollButton.gameObject.SetActive(false);
            holdButton.gameObject.SetActive(false);
            if (EventSystem.current) EventSystem.current.SetSelectedGameObject(null);
            
            PokerInputs.TriggerHold();
        }
        
        private async UniTask OnEndPressed()
        {
            if (EventSystem.current) EventSystem.current.SetSelectedGameObject(null);

            if (LevelManager.Instance)
            {
                LevelManager.Instance.UnloadSceneAsync(pokerDiceGameController.PokerAssetReference.AssetGUID).Forget();
                await LevelManager.Instance.LoadSceneAsync(pokerDiceGameController.GameAssetReference);
            }

            if (InputManager.Instance) InputManager.Instance.EnableOnlyUIMap();
            if (GameManager.Instance) GameManager.Instance.RoomPassed();
        }

        private void DisableGameButtons()
        {
            rollButton.gameObject.SetActive(false);
            holdButton.gameObject.SetActive(false);
        }
        
        private void OnRulesChanged()
        {
            rollButton.gameObject.SetActive(InputRules.CanRoll);
            holdButton.gameObject.SetActive(InputRules.CanHold);
            endButton.gameObject.SetActive(InputRules.CanEnd);
        }
    }
}