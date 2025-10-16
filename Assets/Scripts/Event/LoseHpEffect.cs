using UnityEngine;

namespace Event
{
    [CreateAssetMenu(menuName = "Event Effects/Lose HP")]
    public class LoseHpEffect : EventEffect
    {
        public int Amount;
        public override void Apply(PlayerContext context)
        {
            context.LoseHealth(Amount);
        }
    }
}