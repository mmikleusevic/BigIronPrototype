using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "Combat/Enemy")]
    public class EnemySO : ScriptableObject
    {
        public Enemy prefab;
    }
}