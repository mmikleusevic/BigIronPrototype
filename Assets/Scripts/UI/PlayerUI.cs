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
            
            Unsubscribe(GameManager.Instance.PlayerCombatant);
        }

        private void OnPlayerInitialized()
        {
            Subscribe(GameManager.Instance.PlayerCombatant);
            
            GameManager.Instance.PlayerCombatant.RefreshState();
        }

        protected abstract void Subscribe(PlayerCombatant playerCombatant);
        protected abstract void Unsubscribe(PlayerCombatant playerCombatant);
    }
}