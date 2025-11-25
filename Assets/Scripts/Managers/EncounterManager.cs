using System;
using CombatRoom;
using UnityEngine;

namespace Managers
{
    public class EncounterManager : MonoBehaviour
    {
        public static EncounterManager Instance { get; private set; }

        public EncounterSO EncounterSO { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SetNextEncounter(EncounterSO encounterSO)
        {
            EncounterSO = encounterSO;
        }
    }
}
