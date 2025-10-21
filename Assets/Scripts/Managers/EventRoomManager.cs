using System;
using EventRoom;
using UnityEngine;

namespace Managers
{
    public class EventRoomManager : MonoBehaviour
    {
        public event Action<EventDataSO> OnEventLoaded;
        public event Action OnEventEnded;
        public static EventRoomManager Instance { get; private set; }
    
        private EventDataSO currentEventDataSO; 

        [SerializeField] private GameObject eventRoomPanel;
        
        private void Awake()
        {
            Instance = this;
        }

        public void OnChoiceSelected(EventChoice choice)
        {
            PlayerContext player = GameManager.Instance.PlayerContext;
        
            foreach (EventEffect effect in choice.Effects)
            {
                effect.Apply(player);
            }
        
            if (choice.NextEvent)
            {
                DisplayCurrentEvent(choice.NextEvent);
            }
            else
            {
                EndEventSequence();
            }
        }
    
        public void DisplayCurrentEvent(EventDataSO eventDataSo)
        {
            currentEventDataSO = eventDataSo;
            
            OnEventLoaded?.Invoke(currentEventDataSO);
        }
    
        private void EndEventSequence()
        {
            Debug.Log("Event chain finished!");
            OnEventEnded?.Invoke();
        }
    }
}
