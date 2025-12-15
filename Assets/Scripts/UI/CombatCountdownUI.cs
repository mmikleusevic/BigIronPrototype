using System.Collections;
using CombatRoom;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CombatCountdownUI : MonoBehaviour
    {
        [SerializeField] private GameObject combatCountdownPanel;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private CombatRoomController combatRoomController;
        [SerializeField] private float hideDelayAfterCountdown = 0.5f;
        
        private Tween scaleTween;
        private Coroutine hideRoutine;
        
        private CombatRoomEvents CombatRoomEvents => combatRoomController.CombatRoomEvents;
        
        private void Awake()
        {
            combatCountdownPanel.SetActive(false);
        }

        private void OnEnable()
        {
            CombatRoomEvents.OnCountdownTick += UpdateCountdown;
        }

        private void OnDisable()
        {
            CombatRoomEvents.OnCountdownTick -= UpdateCountdown;
            CleanupAnimations();
        }
        
        private void OnDestroy()
        {
            CleanupAnimations();
        }

        private void UpdateCountdown(int value)
        {
            combatCountdownPanel.SetActive(true);

            if (value == 0)
            {
                countdownText.text = "SHOOT!";
                
                if (hideRoutine != null) StopCoroutine(hideRoutine);
                hideRoutine = StartCoroutine(HidePanelAfterDelay());
            }
            else
            {
                countdownText.text = value.ToString();
            }

            PlayPopAnimation();
        }
        
        private void PlayPopAnimation()
        {
            scaleTween?.Kill();

            if (!countdownText || !countdownText.transform) return;

            Transform tweenTransform = countdownText.transform;

            tweenTransform.localScale = Vector3.one;
            tweenTransform.localRotation = Quaternion.identity;

            scaleTween = DOTween.Sequence()
                .Append(
                    tweenTransform.DOScale(0f, 1f)
                        .SetEase(Ease.OutQuad)
                )
                .Join(
                    tweenTransform.DOLocalRotate(
                        new Vector3(0f, 0f, -360f),
                        1f,
                        RotateMode.FastBeyond360
                    ).SetEase(Ease.OutQuad)
                )
                .SetLink(gameObject);
        }
        
        private IEnumerator HidePanelAfterDelay()
        {
            yield return new WaitForSeconds(hideDelayAfterCountdown);
            
            if (combatCountdownPanel) combatCountdownPanel.SetActive(false);
            
            hideRoutine = null;
        }
        
        private void CleanupAnimations()
        {
            scaleTween?.Kill();
            scaleTween = null;

            if (hideRoutine == null) return;
            
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
    }
}