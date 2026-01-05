using System;
using Managers;
using MapRoom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EventRoom
{
    public class ChoiceButton : EventButton
    {
        public override void Initialize(EventChoice eventChoice)
        {
            onClick.RemoveAllListeners();
            AddListener(eventChoice);
            GetComponentInChildren<TextMeshProUGUI>().text = eventChoice.ChoiceText;
            gameObject.SetActive(true);
        }
        
        protected override void AddListener(EventChoice eventChoice)
        {
            onClick.AddListener(() =>
            {
                if (EventRoomManager.Instance) EventRoomManager.Instance.OnChoiceSelected(eventChoice);
            });
        }
    }
}