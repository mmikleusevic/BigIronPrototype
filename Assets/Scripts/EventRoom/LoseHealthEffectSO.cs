using Player;
using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Effects/LoseHPSO")]
    public class LoseHealthEffectSO : EventEffectSO
    {
        public int Amount;
        public override string Apply(PlayerContext context)
        {
            context.LoseHealth(Amount);
            
            return $"You lost {Amount} HP.";
        }
    }
}