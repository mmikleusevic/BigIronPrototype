namespace Targets
{
    public class TargetScaleModifier : ITargetModifier
    {
        private readonly float multiplier;

        public TargetScaleModifier(float multiplier)
        {
            this.multiplier = multiplier;
        }

        public void Apply(ref TargetStats stats)
        {
            stats.scale *= multiplier;
        }
    }
}