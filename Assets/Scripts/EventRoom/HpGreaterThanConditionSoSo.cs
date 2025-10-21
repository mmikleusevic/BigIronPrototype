using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Conditions/HPGreaterThan")]
    public class HpGreaterThanConditionSoSo : EventConditionSO
    {
        public int Threshold;
        public override bool IsSatisfied(PlayerContext context) => context.CurrentHealth > Threshold;
    }
}