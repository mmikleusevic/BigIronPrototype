using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private GameObject enemyUIPanel;
        [SerializeField] private Slider healthBar;
        [SerializeField] private EnemyHealth health;

        private void Start()
        {
            UpdateHealthBar(health.CurrentHealth, health.MaxHealth);
            
            Hide();
        }

        private void OnEnable()
        {
            health.OnHealthChanged += UpdateHealthBar;
        }

        private void OnDisable()
        {
            health.OnHealthChanged -= UpdateHealthBar;
        }

        public void Show()
        {
            if (enemyUIPanel) enemyUIPanel.SetActive(true);
        }

        public void Hide()
        {
            if (enemyUIPanel) enemyUIPanel.SetActive(false);
        }

        private void UpdateHealthBar(int current, int max)
        {
            healthBar.value = (float)current / max;
        }
    }
}