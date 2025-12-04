using System;
using CombatRoom;
using Cysharp.Threading.Tasks;
using Extensions;
using Managers;
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
        [SerializeField] private Button endButton;
        [SerializeField] private CombatRoomController combatRoomController;
        
        private CombatRoomEvents combatRoomEvents => combatRoomController.CombatRoomEvents;
        private CombatTargetInputs combatTargetInputs => combatRoomController.CombatTargetInputs;
        
        private void Awake()
        {
            endButton.gameObject.SetActive(false);
            combatRoomPanel.SetActive(false);
        }

        private void OnEnable()
        {
            shootButton.onClick.AddListener(Shoot);
            confirmButton.onClick.AddListener(Confirm);
            cancelButton.onClick.AddListener(Cancel);
            endButton.AddClickAsync(End);
            combatRoomEvents.OnPlayerTurnStarted += Show;
            combatRoomEvents.OnPlayerTurnEnded += HideShootButton;
            combatRoomEvents.OnPlayerTargetSelectingStarted += ShowTargetSelectingButtons;
            combatRoomEvents.OnPlayerAttackStarted += Hide;
            combatRoomEvents.OnVictoryStarted += ShowContinueButton;
        }

        private void OnDisable()
        {
            shootButton.onClick.RemoveListener(Shoot);
            confirmButton.onClick.RemoveListener(Confirm);
            cancelButton.onClick.RemoveListener(Cancel);
            endButton.onClick.RemoveAllListeners();
            combatRoomEvents.OnPlayerTurnStarted -= Show;
            combatRoomEvents.OnPlayerTurnEnded -= HideShootButton;
            combatRoomEvents.OnPlayerTargetSelectingStarted -= ShowTargetSelectingButtons;
            combatRoomEvents.OnPlayerAttackStarted -= Hide;
            combatRoomEvents.OnVictoryStarted -= ShowContinueButton;
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

        private async UniTask End()
        {
            if (LevelManager.Instance)
            {
                LevelManager.Instance.UnloadSceneAsync(combatRoomController.CombatRoomAssetReference.AssetGUID).Forget();
                await LevelManager.Instance.LoadSceneAsync(combatRoomController.GameAssetReference);
            }

            if (InputManager.Instance) InputManager.Instance.EnableOnlyUIMap();
            if (GameManager.Instance) GameManager.Instance.RoomPassed();
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

        private void Hide()
        {
            combatRoomPanel.SetActive(false);
        }
    }
}