using System;
using Managers;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerHealthUI : PlayerUI
    {
        [SerializeField] private TextMeshProUGUI healthText;

        protected override void Subscribe(PlayerCombatant playerCombatant) => playerCombatant.Health.OnHealthChanged += UpdateUI;

        protected override void Unsubscribe(PlayerCombatant playerCombatant) => playerCombatant.Health.OnHealthChanged -= UpdateUI;

        private void UpdateUI(int currentHealth, int maxHealth)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}