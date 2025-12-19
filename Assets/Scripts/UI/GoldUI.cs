using DG.Tweening;
using Managers;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GoldUI : PlayerUI
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] protected float animationDuration = 0.3f;

        private int lastGold;
        private Tween goldTween;

        protected override void Subscribe(PlayerCombatant playerCombatant)
        {
            if (playerCombatant && playerCombatant.Gold) playerCombatant.Gold.OnGoldChanged += UpdateUI;
        }

        protected override void Unsubscribe(PlayerCombatant playerCombatant)
        {
            if (playerCombatant && playerCombatant.Gold) playerCombatant.Gold.OnGoldChanged -= UpdateUI;
        }

        private void UpdateUI(int goldAmount)
        {
            goldTween?.Kill();

            int startValue = lastGold;
            int endValue = goldAmount;

            lastGold = endValue;
            
            int delta = endValue - startValue;

            goldTween = DOTween.To(() => startValue, value =>
                {
                    startValue = value;
                    goldText.text = value.ToString();
                },
                endValue,
                animationDuration
            ).SetEase(Ease.OutCubic);

            AnimateFeedback(delta);
        }
        
        private void AnimateFeedback(int delta)
        {
            goldText.DOKill();
            goldText.transform.DOKill();

            if (delta > 0)
            {
                goldText.DOColor(GoldGainColor, 0.3f).SetLoops(2, LoopType.Yoyo);
                goldText.transform.DOPunchScale(Vector3.one * 0.15f, 0.25f);
            }
            else if (delta < 0)
            {
                goldText.DOColor(GoldLossColor, 0.3f).SetLoops(2, LoopType.Yoyo);
                goldText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
            }
        }
        
        private static readonly Color GoldGainColor = new Color(1f, 0.85f, 0.25f);
        private static readonly Color GoldLossColor = new Color(1f, 0.35f, 0.35f);
    }
}