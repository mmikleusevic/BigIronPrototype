namespace Targets
{
    public struct TargetStats
    {
        public float travelDistance;
        public float speed;
        public float scale;
        public float lifetime;

        public static TargetStats FromProfile(TargetProfileSO profile)
        {
            return new TargetStats
            {
                travelDistance = profile.travelDistance,
                speed = profile.speed,
                scale = profile.scale,
                lifetime = profile.lifetime
            };
        }
    }
}