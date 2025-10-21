using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(fileName = "EventEffect", menuName = "Scriptable Objects/EventEffect")]
    public abstract class EventEffect : ScriptableObject
    {
        public abstract void Apply(PlayerContext context);
    }
}
