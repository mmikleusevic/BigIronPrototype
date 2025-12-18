using System.Collections;
using CombatRoom;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : Health
    {
        protected override void Die(Combatant damager, Combatant receiver)
        {
            StartCoroutine(EnemyDeath(damager, receiver));
        }

        private IEnumerator EnemyDeath(Combatant damager, Combatant receiver)
        {
            receiver.CombatantAnimator.SetTrigger(GameStrings.DEATH);

            yield return new WaitForSeconds(2);
            
            Destroy(gameObject);
            
            base.Die(damager, receiver);
        }
    }
}