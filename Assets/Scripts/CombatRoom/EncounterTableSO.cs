using System.Collections.Generic;
using UnityEngine;

namespace CombatRoom
{
    [CreateAssetMenu(menuName = "Combat/EncounterTable")]
    public class EncounterTableSO : ScriptableObject
    {
        public List<EncounterSO> encounters;
    }
}