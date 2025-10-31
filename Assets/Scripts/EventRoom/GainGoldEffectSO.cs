using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Effects/GainGoldSO")]
    public class GainGoldEffectSO : EventEffectSO
    {
        public int Amount;
        public override string Apply(PlayerContext context)
        {
            context.GainGoldAmount(Amount);

            return $"You gained {Amount} gold.";
        }
    }
}
