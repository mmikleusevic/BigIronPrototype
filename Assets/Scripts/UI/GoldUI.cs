using Managers;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GoldUI : PlayerUI
    {
        [SerializeField] private TextMeshProUGUI goldText;

        protected override void Subscribe(PlayerCombatant playerCombatant) => playerCombatant.Gold.OnGoldChanged += UpdateUI;

        protected override void Unsubscribe(PlayerCombatant playerCombatant) => playerCombatant.Gold.OnGoldChanged -= UpdateUI;

        private void UpdateUI(int goldAmount)
        {
            goldText.text = goldAmount.ToString();
        }
    }
}