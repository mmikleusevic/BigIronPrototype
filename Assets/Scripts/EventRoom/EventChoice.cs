using System;
using UnityEngine;

namespace EventRoom
{
    [Serializable]
    public class EventChoice
    {
        public string ChoiceText;
        public EventConditionSO[] Conditions;
        public EventEffect[] Effects;
        public EventDataSO NextEvent;
    }
}