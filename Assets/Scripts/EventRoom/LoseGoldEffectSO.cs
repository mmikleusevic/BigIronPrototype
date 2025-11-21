using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Effects/LoseGoldSO")]
    public class LoseGoldEffectSO : EventEffectSO
    {
        public int Amount;
        public override string Apply(PlayerContext playerContext)
        {
            playerContext.LoseGoldAmount(Amount);

            return $"You lost {Amount} gold.";
        }
    }
}