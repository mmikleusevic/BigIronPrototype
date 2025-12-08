using System.Threading;
using Cysharp.Threading.Tasks;

namespace StateMachine
{
    public interface IState
    {
        public UniTask OnEnter(CancellationToken externalToken);
        public void OnUpdate();
        public UniTask OnExit();
    }
}