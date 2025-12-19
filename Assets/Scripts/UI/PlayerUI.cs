using DG.Tweening;
using Managers;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class PlayerUI : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            if (GameManager.Instance) GameManager.Instance.OnGameStarted += OnGameStarted;
        }

        protected virtual void OnDisable()
        {
            if (GameManager.Instance) GameManager.Instance.OnGameStarted -= OnGameStarted;
            
            Unsubscribe(GameManager.Instance?.PlayerCombatant);
        }

        private void OnGameStarted()
        {
            Subscribe(GameManager.Instance?.PlayerCombatant);

            if (GameManager.Instance) GameManager.Instance.PlayerCombatant.RefreshState();
        }

        protected abstract void Subscribe(PlayerCombatant playerCombatant);
        protected abstract void Unsubscribe(PlayerCombatant playerCombatant);
    }
}