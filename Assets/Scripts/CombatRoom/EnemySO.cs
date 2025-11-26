using UnityEngine;
using Enemies;

namespace CombatRoom
{
    [CreateAssetMenu(menuName = "Combat/Enemy")]
    public class EnemySO : ScriptableObject
    {
        public Enemy prefab;
    }
}