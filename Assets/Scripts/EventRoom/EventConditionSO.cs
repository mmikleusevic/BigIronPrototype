using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(fileName = "EventCondition", menuName = "Scriptable Objects/EventCondition")]
    public abstract class EventConditionSO : ScriptableObject
    {
        public abstract bool IsSatisfied(PlayerContext context);
        public virtual string GetFailReason() => "Condition not met.";
    }
}
