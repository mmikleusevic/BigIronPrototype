using UnityEngine;

namespace Event
{
    [CreateAssetMenu(menuName = "Event Conditions/HP Greater Than")]
    public class HpGreaterThanCondition : EventCondition
    {
        public int Threshold;
        public override bool IsSatisfied(PlayerContext context) => context.CurrentHealth > Threshold;
    }
}