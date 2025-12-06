using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [field: SerializeField] public Health Health { get; private set; }
        
        
        public void PerformAttack()
        {
            
        }
    }
}