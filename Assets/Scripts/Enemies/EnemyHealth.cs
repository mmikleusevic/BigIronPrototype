using CombatRoom;

namespace Enemies
{
    public class EnemyHealth : Health
    {
        protected override void Die(Combatant damager, Combatant receiver)
        {
            Destroy(gameObject);
            
            base.Die(damager, receiver);
        }
    }
}