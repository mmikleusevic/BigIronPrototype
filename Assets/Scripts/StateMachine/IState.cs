using Cysharp.Threading.Tasks;

namespace StateMachine
{
    public interface IState
    {
        public UniTask OnEnter();
        public void OnUpdate();
        public UniTask OnExit();
    }
}