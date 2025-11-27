using System;

namespace CombatRoom
{
    public class CombatRoomEvents
    {
        public Action OnPlayerTurnStarted;
        public Action OnPlayerTurnEnded;
        public Action OnPlayerTargetSelectingStarted;
        public Action OnPlayerTargetSelectingEnded;
    }
}