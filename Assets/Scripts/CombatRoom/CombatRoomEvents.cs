using System;
using Enemies;

namespace CombatRoom
{
    public class CombatRoomEvents
    {
        public Action OnPlayerTurnStarted;
        public Action OnPlayerTurnEnded;
        public Action OnPlayerTargetSelectingStarted;
        public Action OnPlayerTargetSelectingEnded;
        public Action OnPlayerAttackStarted;
        public Action OnPlayerAttackEnded;
        public Action<int> OnCountdownTick;
        public Action<int> OnAttackCountdownTick;
        public Action OnVictoryStarted;
        public Action OnDefeatStarted;
        public Action<string> OnTurnStarted;
    }
}