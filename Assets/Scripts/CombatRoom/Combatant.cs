using Managers;
using Player;
using UnityEngine;

namespace CombatRoom
{
    public abstract class Combatant : MonoBehaviour
    {
        [field: SerializeField] public CombatantDataSO Data { get; protected set; }
        
        [SerializeField] protected Animator combatantAnimator;
        [SerializeField] private Renderer combatantRenderer;
        [SerializeField] private Renderer gunRenderer;
        
        public abstract Health Health { get; }
        public abstract Gold Gold { get; }
        public Animator CombatantAnimator => combatantAnimator;
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
            if (gunRenderer) gunRenderer.enabled = visible;
        }
    }
}