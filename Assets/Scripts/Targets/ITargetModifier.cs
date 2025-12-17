namespace Targets
{
    public interface ITargetModifier
    {
        public void Apply(ref TargetStats stats);
    }
}