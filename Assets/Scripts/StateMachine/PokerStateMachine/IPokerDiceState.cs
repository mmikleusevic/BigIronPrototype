namespace StateMachine.PokerStateMachine
{
    public interface IPokerDiceState
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
    }
}