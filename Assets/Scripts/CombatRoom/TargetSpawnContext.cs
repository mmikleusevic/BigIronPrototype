using UnityEngine;

namespace CombatRoom
{
    public struct TargetSpawnContext
    {
        public TargetProfileSO profile;

        public Vector3 origin;
        public MovementAxis movementAxis;

        public float initialAngle;
        public float movementDistance;
    }
}