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
            GameManager.Instance.OnPlayerReleased += OnPlayerReleased;
        }

        protected virtual void OnDisable()
        {
            GameManager.Instance.OnPlayerInitialized -= OnPlayerInitialized;
            GameManager.Instance.OnPlayerReleased -= OnPlayerReleased;
            
            if (GameManager.Instance.PlayerContext) Unsubscribe(GameManager.Instance.PlayerContext);
        }

        private void OnPlayerInitialized()
        {
            Subscribe(GameManager.Instance.PlayerContext);
            
            GameManager.Instance.PlayerContext.RefreshState();
        }

        private void OnPlayerReleased()
        {
            Unsubscribe(GameManager.Instance.PlayerContext);
        }

        protected abstract void Subscribe(PlayerContext ctx);
        protected abstract void Unsubscribe(PlayerContext ctx);
    }
}