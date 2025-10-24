using System;
using Extensions;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EventRoom
{
    public class EventRoomUI : MonoBehaviour
    {
        [SerializeField] private GameObject eventRoomPanel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button choicePrefab;
        [SerializeField] private Button continueButton;
        [SerializeField] private Transform choiceParent;
        
        private void Awake()
        {
            eventRoomPanel.SetActive(false);
            continueButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (!EventRoomManager.Instance) return;
            
            EventRoomManager.Instance.OnEventLoaded += Initialize;
            EventRoomManager.Instance.OnChoiceResult += ShowChoiceResult;
            EventRoomManager.Instance.OnEventEnded += Hide;
        }

        private void OnDisable()
        {
            if (!EventRoomManager.Instance) return;
            
            EventRoomManager.Instance.OnEventLoaded -= Initialize;
            EventRoomManager.Instance.OnChoiceResult -= ShowChoiceResult;
            EventRoomManager.Instance.OnEventEnded -= Hide;
        }
        
        private void Initialize(EventDataSO eventDataSo)
        {
            RemoveButtons();
            
            titleText.text = eventDataSo.Title;
            descriptionText.text = eventDataSo.Description;

            foreach (EventChoice eventChoice in eventDataSo.Choices)
            {
                Button choiceButton = choicePrefab.GetPooledObject<Button>(choiceParent);
                choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = eventChoice.ChoiceText;
                choiceButton.onClick.AddListener(() =>
                {
                    EventRoomManager.Instance.OnChoiceSelected(eventChoice);
                });
                choiceButton.gameObject.SetActive(true);
            }
            
            eventRoomPanel.SetActive(true);
        }
        
        private void Hide()
        {
            RemoveButtons();
            continueButton.gameObject.SetActive(false);
            eventRoomPanel.SetActive(false);
        }
        
        private void ShowChoiceResult(EventChoice choice, bool conditionsMet)
        {
            RemoveButtons();

            SetDescriptionText(choice, conditionsMet);

            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() =>
            {
                continueButton.gameObject.SetActive(false);
                EventRoomManager.Instance?.ContinueAfterResult(choice);
            });

            continueButton.gameObject.SetActive(true);
        }

        private void SetDescriptionText(EventChoice choice, bool conditionsMet)
        {
            if (conditionsMet)
            {
                descriptionText.text = string.IsNullOrEmpty(choice.SuccessDescription)
                    ? choice.GeneratedResultDescription
                    : choice.SuccessDescription;
            }
            else
            {
                descriptionText.text = choice.FailDescription;
            }
        }
        
        private void RemoveButtons()
        {
            foreach (Transform child in choiceParent)
            {
                if (child == continueButton.transform || child == choicePrefab.transform) continue;
                
                Button button = child.GetComponent<Button>();
                
                if (button) button.onClick.RemoveAllListeners();
                child.ReturnToPool(choicePrefab);
            }
        }
    }
}
