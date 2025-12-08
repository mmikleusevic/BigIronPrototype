using TMPro;
using UnityEngine;

namespace CombatRoom
{
    public class Combo : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private float moveUpSpeed = 1f;
        
        [SerializeField] private TextMeshProUGUI comboText;

        private Color originalColor;
        private float timer;

        public void Initialize(int currentHitStreak)
        {
            originalColor = comboText.color;
            comboText.text = "Combo: " + currentHitStreak;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            
            transform.localPosition += Vector3.up * (moveUpSpeed * Time.deltaTime);
            
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            comboText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            
            if (timer >= fadeDuration) Destroy(gameObject);
        }
    }
}