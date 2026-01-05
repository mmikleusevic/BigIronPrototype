using System.Collections;
using CombatRoom;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : Health
    {
        protected override void Die(Combatant damager, Combatant receiver)
        {
            StartCoroutine(PlayerDeath(receiver));
        }

        private IEnumerator PlayerDeath(Combatant receiver)
        {
            if (EventRoomManager.Instance) EventRoomManager.Instance.LockInteractions();
            
            receiver.CombatantAnimator.Play(GameStrings.DEATH);

            yield return new WaitForSeconds(2);
            
            GameManager.Instance.GameOver(false);
        }
    }
}
