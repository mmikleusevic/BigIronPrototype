using DG.Tweening;
using Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private GameObject enemyUIPanel;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Image healthFill;
        [SerializeField] private EnemyHealth health;
        
        private Tween healthTween;
        private Tween flashTween;
        private Tween punchTween;
        
        private Color originalFillColor;
        
        private int lastHealth = -1;

        private void Awake()
        {
            originalFillColor = healthFill.color;
        }
        
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
            if (current == lastHealth) return;
            lastHealth = current;
            
            float targetValue = (float)current / max;

            healthTween?.Kill();
            flashTween?.Kill();
            punchTween?.Kill();
            
            healthFill.color = Color.white;
            
            healthTween = DOTween.To(
                    () => healthBar.value,
                    x => healthBar.value = x,
                    targetValue,
                    0.25f
                )
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    flashTween = healthFill.DOColor(originalFillColor, 0.15f);
                });
            
            enemyUIPanel.transform.localScale = Vector3.one;
            punchTween = enemyUIPanel.transform
                .DOPunchScale(Vector3.one * 0.08f, 0.15f, 6, 0.8f);
        }
    }
}