namespace Targets
{
    public class TargetSpeedModifier : ITargetModifier
    {
        private readonly float multiplier;

        public TargetSpeedModifier(float multiplier)
        {
            this.multiplier = multiplier;
        }

        public void Apply(ref TargetStats stats)
        {
            stats.speed *= multiplier;
        }
    }
}