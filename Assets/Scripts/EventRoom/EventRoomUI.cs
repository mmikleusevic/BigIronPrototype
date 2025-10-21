using System;
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
        [SerializeField] private Transform choiceParent;

        private void Awake()
        {
            eventRoomPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (!EventRoomManager.Instance) return;
            
            EventRoomManager.Instance.OnEventLoaded += Initialize;
            EventRoomManager.Instance.OnEventEnded += Hide;
        }

        private void OnDisable()
        {
            if (!EventRoomManager.Instance) return;
            
            EventRoomManager.Instance.OnEventLoaded -= Initialize;
            EventRoomManager.Instance.OnEventEnded -= Hide;
        }
        
        private void Initialize(EventDataSO eventDataSo)
        {
            titleText.text = eventDataSo.Title;
            descriptionText.text = eventDataSo.Description;

            foreach (EventChoice eventChoice in eventDataSo.Choices)
            {
                Button choiceButton = Instantiate(choicePrefab, choiceParent);
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
            foreach (Transform child in choiceParent)
            {
                Button button = child.GetComponent<Button>();
                
                if (button) button.onClick.RemoveAllListeners();
                Destroy(child.gameObject);
            }
            
            eventRoomPanel.SetActive(false);
        }
    }
}
