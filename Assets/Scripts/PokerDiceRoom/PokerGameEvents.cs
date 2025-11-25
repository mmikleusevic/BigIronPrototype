using System;

namespace PokerDiceRoom
{
    public class PokerGameEvents
    {
        public Action<PokerPlayer> OnTurnStart;
        public Action OnTurnEndStarted;
        public Action<int, int> OnDiceRollingStarted;
        public Action OnRoll;
        public Action OnHold;
        public Action OnDiceRollingEnded;
        public Action OnGameOverStarted;
        public Action OnGameOver;
        public Action OnDiceEvaluationStarted;
    }
}