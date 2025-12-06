using UnityEngine;

namespace CombatRoom
{
    [CreateAssetMenu(menuName = "Target/TargetProfile")]
    public class TargetProfileSO : ScriptableObject
    {
        public float scale = 1f;
        public float lifetime = 1.5f;
        public float speed = 1.0f;
        public float orbitRadius = 1;
    }
}