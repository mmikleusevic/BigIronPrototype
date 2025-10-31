using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(fileName = "EventEffectSO", menuName = "Scriptable Objects/EventEffectSO")]
    public abstract class EventEffectSO : ScriptableObject
    {
        public abstract string Apply(PlayerContext context);
    }
}
