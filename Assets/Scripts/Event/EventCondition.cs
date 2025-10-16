using UnityEngine;

namespace Event
{
    [CreateAssetMenu(fileName = "EventCondition", menuName = "Scriptable Objects/EventCondition")]
    public abstract class EventCondition : ScriptableObject
    {
        public abstract bool IsSatisfied(PlayerContext context);
    }
}
