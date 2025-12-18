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
        [SerializeField] private GameObject combatRoomPanel;
        [SerializeField] private Button shootButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button endButton;
        [SerializeField] private CombatRoomController combatRoomController;
        
        private CombatRoomEvents CombatRoomEvents => combatRoomController.CombatRoomEvents;
        private CombatTargetInputs CombatTargetInputs => combatRoomController.CombatTargetInputs;
        
        private void Awake()
        {
            endButton.gameObject.SetActive(false);
            combatRoomPanel.SetActive(false);
            attackCountdownText.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            shootButton.onClick.AddListener(Shoot);
            confirmButton.onClick.AddListener(Confirm);
            cancelButton.onClick.AddListener(Cancel);
            endButton.AddClickAsync(End);
            CombatRoomEvents.OnPlayerTurnStarted += Show;
            CombatRoomEvents.OnPlayerTurnEnded += HideShootButton;
            CombatRoomEvents.OnPlayerTargetSelectingStarted += ShowTargetSelectingButtons;
            CombatRoomEvents.OnPlayerAttackStarted += Hide;
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
            CombatRoomEvents.OnPlayerTurnStarted -= Show;
            CombatRoomEvents.OnPlayerTurnEnded -= HideShootButton;
            CombatRoomEvents.OnPlayerTargetSelectingStarted -= ShowTargetSelectingButtons;
            CombatRoomEvents.OnPlayerAttackStarted -= Hide;
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
        
        private void Show()
        {
            combatRoomPanel.SetActive(true);
            shootButton.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
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

        private void Hide()
        {
            combatRoomPanel.SetActive(false);
        }
    }
}