using UnityEngine;

namespace CombatRoom
{
    public struct TargetSpawnContext
    {
        public TargetProfileSO profile;
        public Vector3 origin;
        public Transform centerTransform;
        public MovementAxis movementAxis;
    }
}