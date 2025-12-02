using System;
using CombatRoom;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GunUI : MonoBehaviour
    {
        [SerializeField] private GameObject gunPanel;
        [SerializeField] private TextMeshProUGUI gunNameText;
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private CombatRoomController combatRoomController;

        private CombatRoomEvents CombatRoomEvents => combatRoomController.CombatRoomEvents;
        
        private void Awake()
        {
            Hide();
        }
        
        private void OnEnable()
        {
            CombatRoomEvents.OnPlayerAttackStarted += Show;
            CombatRoomEvents.OnPlayerAttackEnded += Hide;
        }

        private void OnDisable()
        {
            CombatRoomEvents.OnPlayerAttackStarted -= Show;
            CombatRoomEvents.OnPlayerAttackEnded -= Hide;
        }

        public void SetGunName(string gunName)
        {
            gunNameText.text = gunName;
        }

        public void SetAmmo(int current, int max)
        {
            ammoText.text = $"{current}/{max}";
        }

        private void Show()
        {
            gunPanel.SetActive(true);
        }
        
        private void Hide()
        {
            gunPanel.SetActive(false);
        }
    }
}