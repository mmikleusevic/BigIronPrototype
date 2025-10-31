using System;
using System.Collections.Generic;
using EventRoom;
using Player;
using UnityEngine;

namespace Managers
{
    public class EventRoomManager : MonoBehaviour
    {
        public event Action<EventSO> OnEventLoaded;
        public event Action<EventChoice, bool> OnChoiceResult;
        public event Action OnEventEnded;
        public static EventRoomManager Instance { get; private set; }
    
        private EventSO currentEventSo; 
        
        private void Awake()
        {
            Instance = this;
        }

        public void OnChoiceSelected(EventChoice choice)
        {
            PlayerContext player = GameManager.Instance?.PlayerContext;

            bool conditionsMet = CheckConditions(choice, player);
            TriggerEffects(choice, player,conditionsMet);
            
            OnChoiceResult?.Invoke(choice, conditionsMet);
        }
        
        public void ContinueAfterResult(EventChoice choice)
        {
            if (choice.NextEvent)
            {
                DisplayCurrentEvent(choice.NextEvent);
            }
            else
            {
                EndEventSequence();
            }
        }

        private bool CheckConditions(EventChoice choice, PlayerContext player)
        {
            bool conditionsMet = true;
            foreach (EventConditionSO eventConditionSO in choice.Conditions)
            {
                conditionsMet = eventConditionSO.IsSatisfied(player);
                if (conditionsMet) continue;
                
                choice.FailDescription = eventConditionSO.GetFailReason();
                break;
            }

            return conditionsMet;
        }

        private void TriggerEffects(EventChoice choice, PlayerContext player , bool conditionsMet)
        {
            if (!conditionsMet) return;
            
            List<string> resultParts = new List<string>();

            foreach (EventEffectSO effect in choice.Effects)
            {
                string result = effect.Apply(player);
                if(!string.IsNullOrEmpty(result)) resultParts.Add(result);
            }
            
            string combinedResult = string.Join(" ", resultParts);
            choice.GeneratedResultDescription = combinedResult;
        }
        
        public void DisplayCurrentEvent(EventSO eventSo)
        {
            currentEventSo = eventSo;
            OnEventLoaded?.Invoke(currentEventSo);
        }
    
        private void EndEventSequence()
        {
            Debug.Log("Event chain finished!");
            OnEventEnded?.Invoke();
        }
    }
}