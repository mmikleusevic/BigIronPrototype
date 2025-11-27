using System;
using CombatRoom;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CombatRoomUI : MonoBehaviour
    {
        public event Action OnShoot;
        
        [SerializeField] private GameObject combatRoomPanel;
        [SerializeField] private Button shootButton;
        [SerializeField] private CombatRoomManager combatRoomManager;
        
        private CombatRoomEvents combatRoomEvents => combatRoomManager.CombatRoomEvents;

        private void Awake()
        {
            combatRoomPanel.SetActive(false);
        }

        private void OnEnable()
        {
            shootButton.onClick.AddListener(Shoot);
            combatRoomEvents.OnPlayerTurnStarted += Show;
            combatRoomEvents.OnPlayerTurnEnded += Hide;
            OnShoot += combatRoomManager.HandleShootChosen;
        }

        private void OnDisable()
        {
            shootButton.onClick.RemoveListener(Shoot);
            combatRoomEvents.OnPlayerTurnStarted -= Show;
            combatRoomEvents.OnPlayerTurnEnded -= Hide;
            OnShoot -= combatRoomManager.HandleShootChosen;
        }

        private void Shoot()
        {
            OnShoot?.Invoke();
        }
        
        private void Show()
        {
            combatRoomPanel.SetActive(true);
        }

        private void Hide()
        {
            combatRoomPanel.SetActive(false);
        }
    }
}