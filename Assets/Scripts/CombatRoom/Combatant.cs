using Player;
using UnityEngine;

namespace CombatRoom
{
    public abstract class Combatant : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public int Speed { get; protected set; }
        [field: SerializeField] public CombatantType Type { get; protected set; }
        [field: SerializeField] public int Damage { get; protected set; }
        public abstract Health Health { get; }
        public abstract Gold Gold { get; }
        public bool IsDead => Health && Health.CurrentHealth <= 0;
    }
}