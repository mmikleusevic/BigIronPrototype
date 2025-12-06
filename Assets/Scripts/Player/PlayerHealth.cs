using CombatRoom;

namespace Player
{
    public class PlayerHealth : Health
    {
        protected override void Die(Combatant damager, Combatant receiver)
        {
            base.Die(damager, receiver);
            
            //TODO trigger game over
        }
    }
}
