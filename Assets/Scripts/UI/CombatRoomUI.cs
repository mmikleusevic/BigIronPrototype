using System;
using CombatRoom;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CombatRoomUI : MonoBehaviour
    {
        [SerializeField] private GameObject combatRoomPanel;
        [SerializeField] private Button shootButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private CombatRoomManager combatRoomManager;
        
        private CombatRoomEvents combatRoomEvents => combatRoomManager.CombatRoomEvents;
        private CombatTargetInputs combatTargetInputs => combatRoomManager.CombatTargetInputs;
        
        private void Awake()
        {
            combatRoomPanel.SetActive(false);
        }

        private void OnEnable()
        {
            shootButton.onClick.AddListener(Shoot);
            confirmButton.onClick.AddListener(Confirm);
            cancelButton.onClick.AddListener(Cancel);
            combatRoomEvents.OnPlayerTurnStarted += Show;
            combatRoomEvents.OnPlayerTurnEnded += HideShootButton;
            combatRoomEvents.OnPlayerTargetSelectingStarted += ShowTargetSelectingButtons;
            combatRoomEvents.OnPlayerAttackStarted += Hide;
        }

        private void OnDisable()
        {
            shootButton.onClick.RemoveListener(Shoot);
            confirmButton.onClick.RemoveListener(Confirm);
            cancelButton.onClick.RemoveListener(Cancel);
            combatRoomEvents.OnPlayerTurnStarted -= Show;
            combatRoomEvents.OnPlayerTurnEnded -= HideShootButton;
            combatRoomEvents.OnPlayerTargetSelectingStarted -= ShowTargetSelectingButtons;
            combatRoomEvents.OnPlayerAttackStarted -= Hide;
        }

        private void Shoot()
        {
            combatTargetInputs.TriggerShootSelected();
        }

        private void Confirm()
        {
            combatTargetInputs.TriggerConfirm();
        }

        private void Cancel()
        {
            combatTargetInputs.TriggerCancel();
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