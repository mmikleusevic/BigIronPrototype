using Player;
using UnityEngine;

namespace CombatRoom
{
    public abstract class Combatant : MonoBehaviour
    {
        [field: SerializeField] public CombatantDataSO Data { get; protected set; }
        [field: SerializeField] public Animator CombatantAnimator { get; protected set; }
        
        [SerializeField] private Renderer combatantRenderer;
        
        public abstract Health Health { get; }
        public abstract Gold Gold { get; }
        public bool IsDead => Health && Health.CurrentHealth <= 0;
        
        public void GainGoldAmount(int amount)
        {
            Gold.GainGoldAmount(amount);
        }

        public int LoseGoldAmount(int amount)
        {
            return Gold.LoseGoldAmount(amount);
        }

        public int TakeDamage(Combatant damager, Combatant receiver, int damage)
        {
            return Health.TakeDamage(damager, receiver, damage);
        }

        public int Heal(int healAmount)
        {
            return Health.Heal(healAmount);
        }
        
        public void ToggleVisibility(bool visible)
        {
            if (combatantRenderer) combatantRenderer.enabled = visible;
        }
    }
}