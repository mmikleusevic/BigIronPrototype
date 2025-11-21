using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : Health
    {
        protected override void Die()
        {
            base.Die();
            
            //TODO kill off enemy
        }
    }
}