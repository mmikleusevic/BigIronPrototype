using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(fileName = "EventEffect", menuName = "Scriptable Objects/EventEffect")]
    public abstract class EventEffectSO : ScriptableObject
    {
        public abstract string Apply(PlayerContext context);
    }
}
