using System;
using DG.Tweening;
using Managers;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerHealthUI : PlayerUI
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private float animationDuration = 0.3f;

        private int lastHealth;
        private Tween healthTween;

        private void OnDestroy()
        {
            healthTween?.Kill();
            healthTween = null;
        }

        protected override void Subscribe(PlayerCombatant playerCombatant)
        {
            if (playerCombatant && playerCombatant.Health) playerCombatant.Health.OnHealthChanged += UpdateUI;
        }

        protected override void Unsubscribe(PlayerCombatant playerCombatant)
        {
            if (playerCombatant && playerCombatant.Health) playerCombatant.Health.OnHealthChanged -= UpdateUI;
        }
        
        private void UpdateUI(int currentHealth, int maxHealth)
        {
            healthTween?.Kill();

            int startValue = lastHealth;
            int endValue = currentHealth;
            
            lastHealth = currentHealth;
            
            healthTween = DOTween.To(() => startValue, value =>
                {
                    startValue = value;
                    healthText.text = $"{value}/{maxHealth}";
                },
                endValue,
                animationDuration
            );

            AnimateFeedback(endValue - startValue);
        }
        
        private void AnimateFeedback(int delta)
        {
            healthText.transform.DOKill();
            healthText.DOKill();

            switch (delta)
            {
                case < 0:
                    healthText.DOColor(Color.red, 0.3f).SetLoops(2, LoopType.Yoyo);
                    healthText.transform.DOPunchScale(Vector3.one * 0.15f, 0.2f);
                    break;
                case > 0:
                    healthText.DOColor(Color.green, 0.3f).SetLoops(2, LoopType.Yoyo);
                    healthText.transform.DOPunchScale(Vector3.one * 0.12f, 0.2f);
                    break;
            }
        }
    }
}