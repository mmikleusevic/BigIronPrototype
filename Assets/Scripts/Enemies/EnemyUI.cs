using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private GameObject enemyUIPanel;
        [SerializeField] private Image healthBar;
        [SerializeField] private EnemyHealth health;
        [SerializeField] private TextMeshProUGUI healthText;

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
            enemyUIPanel.SetActive(true);
        }

        public void Hide()
        {
            enemyUIPanel?.SetActive(false);
        }

        private void UpdateHealthBar(int current, int max)
        {
            healthBar.fillAmount = (float)current / max;
            healthText.text = current.ToString();
        }
    }
}