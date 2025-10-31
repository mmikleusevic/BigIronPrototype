using System;
using UnityEngine;

namespace EventRoom
{
    [Serializable]
    public class EventChoice
    {
        public string ChoiceText;
        public EventConditionSO[] Conditions;
        public EventEffectSO[] Effects;
        public EventSO NextEvent;
        
        [NonSerialized] public string GeneratedResultDescription;
        [TextArea] public string SuccessDescription;
        [NonSerialized] public string FailDescription;
    }
}