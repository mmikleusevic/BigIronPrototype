using System;

namespace Event
{
    [Serializable]
    public class EventChoice
    {
        public string ChoiceText;
        public EventCondition[] Conditions;
        public EventEffect[] Effects;
        public EventData NextEvent;
    }
}