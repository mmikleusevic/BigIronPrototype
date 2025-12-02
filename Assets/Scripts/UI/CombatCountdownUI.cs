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
        
        private CombatRoomEvents CombatRoomEvents => combatRoomController.CombatRoomEvents;
        private Coroutine hideRoutine;
        
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
            
            countdownText.transform.localScale = Vector3.one;
            
            scaleTween = countdownText.transform
                .DOScale(0, 1f)
                .SetEase(Ease.OutQuad);
        }
        
        private IEnumerator HidePanelAfterDelay()
        {
            yield return new WaitForSeconds(hideDelayAfterCountdown);
            combatCountdownPanel.SetActive(false);
        }
    }
}