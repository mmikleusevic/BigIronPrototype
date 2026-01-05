using System;
using CombatRoom;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class CombatRoomUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI attackCountdownText;
        [SerializeField] private TextMeshProUGUI biggestComboThisTurnText;
        [SerializeField] private TextMeshProUGUI currentCombatantsNameText;
        [SerializeField] private GameObject combatRoomPanel;
        [SerializeField] private Button shootButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button endButton;
        [SerializeField] private CombatRoomController combatRoomController;
        
        private CombatRoomEvents CombatRoomEvents => combatRoomController.CombatRoomEvents;
        private CombatTargetInputs CombatTargetInputs => combatRoomController.CombatTargetInputs;
        private PlayerComboSystem PlayerComboSystem => combatRoomController.PlayerComboSystem;
        
        private Tween comboTween;
        private Tween combatantTween;
        private Color comboOriginalColor;
        
        private void Awake()
        {
            endButton.gameObject.SetActive(false);
            combatRoomPanel.SetActive(false);
            attackCountdownText.gameObject.SetActive(false);
            biggestComboThisTurnText.gameObject.SetActive(false);
            currentCombatantsNameText.gameObject.SetActive(false);
            
            comboOriginalColor = biggestComboThisTurnText.color;
        }

        private void OnEnable()
        {
            shootButton.onClick.AddListener(Shoot);
            confirmButton.onClick.AddListener(Confirm);
            cancelButton.onClick.AddListener(Cancel);
            endButton.AddClickAsync(End);
            CombatRoomEvents.OnTurnStarted += ShowCurrentCombatant;
            CombatRoomEvents.OnPlayerTurnStarted += Show;
            CombatRoomEvents.OnPlayerTurnEnded += HideShootButton;
            CombatRoomEvents.OnPlayerTargetSelectingStarted += ShowTargetSelectingButtons;
            CombatRoomEvents.OnPlayerAttackStarted += Hide;
            CombatRoomEvents.OnPlayerAttackEnded += HideCombo;
            PlayerComboSystem.OnNewHitStreak += SetNewCombo;
            CombatRoomEvents.OnVictoryStarted += ShowContinueButton;
            CombatRoomEvents.OnDefeatStarted += Hide;
            CombatRoomEvents.OnAttackCountdownTick += UpdateAttackCountdownText;
        }

        private void OnDisable()
        {
            shootButton.onClick.RemoveListener(Shoot);
            confirmButton.onClick.RemoveListener(Confirm);
            cancelButton.onClick.RemoveListener(Cancel);
            endButton.onClick.RemoveAllListeners();
            CombatRoomEvents.OnTurnStarted -= ShowCurrentCombatant;
            CombatRoomEvents.OnPlayerTurnStarted -= Show;
            CombatRoomEvents.OnPlayerTurnEnded -= HideShootButton;
            CombatRoomEvents.OnPlayerTargetSelectingStarted -= ShowTargetSelectingButtons;
            CombatRoomEvents.OnPlayerAttackStarted -= Hide;
            CombatRoomEvents.OnPlayerAttackEnded -= HideCombo;
            PlayerComboSystem.OnNewHitStreak -= SetNewCombo;
            CombatRoomEvents.OnVictoryStarted -= ShowContinueButton;
            CombatRoomEvents.OnDefeatStarted -= Hide;
            CombatRoomEvents.OnAttackCountdownTick -= UpdateAttackCountdownText;
        }

        private void Shoot()
        {
            CombatTargetInputs.TriggerShootSelected();
        }

        private void Confirm()
        {
            CombatTargetInputs.TriggerConfirm();
        }

        private void Cancel()
        {
            CombatTargetInputs.TriggerCancel();
        }

        private async UniTask End()
        {
            if (!LevelManager.Instance) return; 
            
            await LevelManager.Instance.UnloadSceneActivateGame(combatRoomController.CombatRoomAssetReference);
        }
        
        private void HideShootButton()
        {
            shootButton.gameObject.SetActive(false);
        }

        private void ShowTargetSelectingButtons()
        {
            confirmButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
        }

        private void ShowContinueButton()
        {
            combatRoomPanel.SetActive(true);
            endButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(false);
            shootButton.gameObject.SetActive(false);
        }
        
        private void ShowCurrentCombatant(string combatantsName)
        {
            currentCombatantsNameText.text = combatantsName + "'s Turn";
            currentCombatantsNameText.gameObject.SetActive(true);

            combatantTween?.Kill();

            combatantTween = DOTween.Sequence()
                .Append(currentCombatantsNameText.transform
                    .DOPunchScale(Vector3.one * 0.2f, 0.5f, 6, 0.8f))
                .Join(currentCombatantsNameText.DOFade(1f, 0.5f))
                .AppendInterval(1f)
                .Append(currentCombatantsNameText.DOFade(0f, 0.5f))
                .OnComplete(() => currentCombatantsNameText.gameObject.SetActive(false));
        }
        
        private void HideCombo()
        {
            biggestComboThisTurnText.gameObject.SetActive(false);
        }
        
        private void SetNewCombo(int newCombo)
        {
            biggestComboThisTurnText.gameObject.SetActive(true);
            biggestComboThisTurnText.text = $"Combo: {newCombo}";

            comboTween?.Kill();

            biggestComboThisTurnText.transform.localScale = Vector3.one;
            biggestComboThisTurnText.color = comboOriginalColor;

            float punchStrength = Mathf.Clamp(0.15f + newCombo * 0.01f, 0.15f, 0.35f);
            
            comboTween = DOTween.Sequence()
                .Append(biggestComboThisTurnText.transform.DOPunchScale(Vector3.one * punchStrength, 0.25f, 6, 0.8f))
                .Join(biggestComboThisTurnText.DOColor(Color.red, 0.12f))
                .Append(biggestComboThisTurnText.DOColor(comboOriginalColor, 0.18f));
        }
        
        private void UpdateAttackCountdownText(int seconds)
        {
            attackCountdownText.gameObject.SetActive(true);
            
            if (seconds <= 0)
            {
                attackCountdownText.gameObject.SetActive(false);
            }
            if (seconds <= 3f)
            {
                attackCountdownText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
                attackCountdownText.DOColor(Color.red, 0.15f).SetLoops(2, LoopType.Yoyo);
            }
            else
            {
                attackCountdownText.gameObject.SetActive(true);
            }
            
            attackCountdownText.text = seconds.ToString();
        }
        
        private void Show()
        {
            combatRoomPanel.SetActive(true);
            shootButton.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
        }

        private void Hide()
        {
            combatRoomPanel.SetActive(false);
        }
    }
}