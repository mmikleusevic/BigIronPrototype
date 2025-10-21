using System;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(fileName = "EventData", menuName = "Scriptable Objects/EventData")]
    public class EventDataSO : ScriptableObject
    {
        public string Title;
        [TextArea] public string Description;
        public EventChoice[] Choices;
    }
}
