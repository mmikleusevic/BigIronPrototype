using Extensions;
using UnityEngine.UI;

namespace EventRoom
{
    public abstract class EventButton : Button
    {
        protected override void OnDisable()
        {
            onClick.RemoveAllListeners();
        }

        public abstract void Initialize(EventChoice eventChoice);
        
        protected abstract void AddListener(EventChoice eventChoice);
    }
}