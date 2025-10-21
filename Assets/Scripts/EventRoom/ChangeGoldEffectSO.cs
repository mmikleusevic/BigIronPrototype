using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Effects/GainGold")]
    public class ChangeGoldEffectSO : EventEffect
    {
        public int Amount;
        public override void Apply(PlayerContext context)
        {
            context.ChangeGoldAmount(Amount);
        }
    }
}
