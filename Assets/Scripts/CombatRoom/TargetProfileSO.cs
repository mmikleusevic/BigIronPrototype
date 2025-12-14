using UnityEngine;

namespace CombatRoom
{
    [CreateAssetMenu(menuName = "Target/TargetProfile")]
    public class TargetProfileSO : ScriptableObject
    {
        public float travelDistance = 3f;
        public float speed = 1f;
        public float scale = 1f;
        public float lifetime = 1.5f;
        public float orbitRadius = 1;
    }
}