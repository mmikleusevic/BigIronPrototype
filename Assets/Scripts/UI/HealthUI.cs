using System;
using Managers;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HealthUI : PlayerUI
    {
        [SerializeField] private TextMeshProUGUI healthText;

        protected override void Subscribe(PlayerContext ctx) => ctx.OnHealthChanged += UpdateUI;

        protected override void Unsubscribe(PlayerContext ctx) => ctx.OnHealthChanged -= UpdateUI;

        private void UpdateUI(int currentHealth, int maxHealth)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}