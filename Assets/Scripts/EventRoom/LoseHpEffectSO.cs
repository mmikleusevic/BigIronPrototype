using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Effects/LoseHP")]
    public class LoseHpEffectSO : EventEffect
    {
        public int Amount;
        public override void Apply(PlayerContext context)
        {
            context.LoseHealth(Amount);
        }
    }
}