using Managers;
using Player;
using UnityEngine;

namespace UI
{
    public abstract class PlayerUI : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            GameManager.Instance.OnPlayerInitialized += OnPlayerInitialized;
        }

        protected virtual void OnDisable()
        {
            GameManager.Instance.OnPlayerInitialized -= OnPlayerInitialized;
            
            Unsubscribe(GameManager.Instance.PlayerContext);
        }

        private void OnPlayerInitialized()
        {
            Subscribe(GameManager.Instance.PlayerContext);
            
            GameManager.Instance.PlayerContext.RefreshState();
        }

        protected abstract void Subscribe(PlayerContext ctx);
        protected abstract void Unsubscribe(PlayerContext ctx);
    }
}