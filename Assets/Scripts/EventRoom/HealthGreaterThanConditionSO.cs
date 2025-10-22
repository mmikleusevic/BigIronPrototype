using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Conditions/HPGreaterThan")]
    public class HealthGreaterThanConditionSO : EventConditionSO
    {
        public int Threshold;
        public override bool IsSatisfied(PlayerContext context) => context.CurrentHealth > Threshold;
        public override string GetFailReason() => $"Requires more than {Threshold} health.";
    }
}