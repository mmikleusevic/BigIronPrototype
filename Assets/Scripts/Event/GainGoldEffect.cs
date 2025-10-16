using UnityEngine;

namespace Event
{
    [CreateAssetMenu(menuName = "Event Effects/Gain Gold")]
    public class GainGoldEffect : EventEffect
    {
        public int Amount;
        public override void Apply(PlayerContext context)
        {
            context.ChangeGoldAmount(Amount);
        }
    }
}
