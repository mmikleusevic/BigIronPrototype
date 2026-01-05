using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace EventRoom
{
    public class ContinueButton : EventButton
    {
        public override void Initialize(EventChoice eventChoice)
        {
            onClick.RemoveAllListeners();
            AddListener(eventChoice);
            gameObject.SetActive(true);
        }

        protected override void AddListener(EventChoice eventChoice)
        {
            onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                if (EventRoomManager.Instance) EventRoomManager.Instance.ContinueAfterResult(eventChoice);
                onClick.RemoveAllListeners();
            });
        }
    }
}