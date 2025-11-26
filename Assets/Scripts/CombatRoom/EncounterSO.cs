using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace CombatRoom
{
    [CreateAssetMenu(menuName = "Combat/Encounter")]
    public class EncounterSO : ScriptableObject
    {
        public EnemyCombatant[] enemies;
    }
}