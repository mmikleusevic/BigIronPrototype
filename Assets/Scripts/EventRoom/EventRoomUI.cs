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
        [SerializeField] private ChoiceButton choicePrefab;
        [SerializeField] private ContinueButton continueButton;
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
        
        private void Initialize(EventSO eventSo)
        {
            titleText.text = eventSo.Title;
            descriptionText.text = eventSo.Description;

            foreach (EventChoice eventChoice in eventSo.Choices)
            {
                ChoiceButton choiceButton = Instantiate(choicePrefab, choicePrefab.transform.position, choicePrefab.transform.rotation, choiceParent);
                choiceButton.Initialize(eventChoice);
            }
            
            eventRoomPanel.SetActive(true);
        }
        
        private void Hide()
        {
            continueButton.gameObject.SetActive(false);
            eventRoomPanel.SetActive(false);
        }
        
        private void ShowChoiceResult(EventChoice choice, bool conditionsMet)
        {
            DestroyButtons();
            
            SetDescriptionText(choice, conditionsMet);
            RegisterContinueButton(choice);
        }

        private void RegisterContinueButton(EventChoice eventChoice)
        {
            continueButton.Initialize(eventChoice);
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

        private void DestroyButtons()
        {
            for (int i = 0; i < choiceParent.childCount; i++)
            {
                if (i < 2) continue;
                
                Transform child = choiceParent.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }
}
