using UnityEngine;

namespace CombatRoom
{
    [CreateAssetMenu(menuName = "Target/TargetProfile")]
    public class TargetProfileSO : ScriptableObject
    {
        public float radius = 1f;
        public float lifetime = 1.5f;
    }
}