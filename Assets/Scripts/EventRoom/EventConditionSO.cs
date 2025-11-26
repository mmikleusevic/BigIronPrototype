using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(fileName = "EventConditionSO", menuName = "Scriptable Objects/EventConditionSO")]
    public abstract class EventConditionSO : ScriptableObject
    {
        public abstract bool IsSatisfied(PlayerCombatant context);
        public virtual string GetFailReason() => "Condition not met.";
    }
}
