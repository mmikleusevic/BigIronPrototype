using CombatRoom;
using UnityEngine;

namespace Targets
{
    public struct TargetSpawnContext
    {
        public TargetProfileSO profile;
        public Vector3 origin;
        public Transform centerTransform;
        public TargetMovementAxis targetMovementAxis;
    }
}