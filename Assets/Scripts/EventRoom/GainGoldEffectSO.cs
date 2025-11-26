using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Effects/GainGoldSO")]
    public class GainGoldEffectSO : EventEffectSO
    {
        public int Amount;
        public override string Apply(PlayerCombatant playerContext)
        {
            playerContext.GainGoldAmount(Amount);

            return $"You gained {Amount} gold.";
        }
    }
}
