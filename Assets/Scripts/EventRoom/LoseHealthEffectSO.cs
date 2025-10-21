using UnityEngine;

namespace EventRoom
{
    [CreateAssetMenu(menuName = "Event Effects/LoseHP")]
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