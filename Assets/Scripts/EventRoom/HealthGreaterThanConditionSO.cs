using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Conditions/HPGreaterThanSO")]
    public class HealthGreaterThanConditionSO : EventConditionSO
    {
        public int Threshold;
        public override bool IsSatisfied(PlayerCombatant context) => context.Health.CurrentHealth > Threshold;
        public override string GetFailReason() => $"Requires more than {Threshold} health.";
    }
}