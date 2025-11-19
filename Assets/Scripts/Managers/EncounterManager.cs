using System;
using CombatRoom;
using UnityEngine;

namespace Managers
{
    public class EncounterManager : MonoBehaviour
    {
        public static EncounterManager Instance { get; private set; }

        private EncounterSO encounterSO;

        private void Awake()
        {
            Instance = this;
        }

        public void SetNextEncounter(EncounterSO encounterSO)
        {
            this.encounterSO = encounterSO;
        }

        public void InitializeEncounter()
        {
            //TODO initialize encounter
        }
    }
}
