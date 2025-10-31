namespace PokerDiceRoom
{
    public class PokerInputRules
    {
        public bool CanRoll { get; private set; }
        public bool CanHold { get; private set; }
        public bool CanContinue { get; private set; }

        public void SetRollingPhase(int currentRoll, int maxRolls)
        {
            CanRoll = currentRoll < maxRolls;
            CanHold = currentRoll > 0 && currentRoll < maxRolls;
            CanContinue = false;
        }

        public void SetEvaluationPhase()
        {
            CanRoll = false;
            CanHold = false;
            CanContinue = true;
        }

        public void Reset()
        {
            CanRoll = false;
            CanHold = false;
            CanContinue = false;
        }
    }
}