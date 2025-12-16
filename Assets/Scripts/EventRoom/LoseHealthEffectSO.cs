using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Effects/LoseHealthSO")]
    public class LoseHealthEffectSO : EventEffectSO
    {
        public int Amount;
        
        public override string Apply(PlayerCombatant playerContext)
        {
            int lostHealth = playerContext.TakeDamage(null, null, Amount);
            
            return $"You lost {lostHealth} Health.";
        }
    }
}