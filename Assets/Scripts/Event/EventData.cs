using UnityEngine;

namespace Event
{
    [CreateAssetMenu(fileName = "EventData", menuName = "Scriptable Objects/EventData")]
    public class EventData : ScriptableObject
    {
        public string Title;
        [TextArea] public string Description;
        public EventChoice[] Choices;
    }
}
