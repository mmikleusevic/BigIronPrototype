using UnityEngine;

namespace CombatRoom
{
    [CreateAssetMenu(menuName = "Combatant/Data")]
    public class CombatantDataSO : ScriptableObject
    {
        public string combatantName;
        public float speed;
        public CombatantType combatantType;
        public int damage;
    }
}