using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Effects/GainHealthSO")]
    public class GainHealthEffectSO : EventEffectSO
    {
        public int Amount;
        
        public override string Apply(PlayerCombatant playerContext)
        {
            int gainedHealth = playerContext.Heal(Amount);
            
            return $"You gained {gainedHealth} Health.";
        }
    }
}