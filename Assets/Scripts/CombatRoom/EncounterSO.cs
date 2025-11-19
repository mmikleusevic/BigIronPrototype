using System.Collections.Generic;
using UnityEngine;

namespace CombatRoom
{
    [CreateAssetMenu(menuName = "Combat/Encounter")]
    public class EncounterSO : ScriptableObject
    {
        public List<EnemySO> enemies;
        public int difficulty;
    }
}