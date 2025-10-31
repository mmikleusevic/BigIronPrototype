using System;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(fileName = "EventSO", menuName = "Scriptable Objects/EventSO")]
    public class EventSO : ScriptableObject
    {
        public string Title;
        public EventChoice[] Choices;
        [TextArea] public string Description;
    }
}
