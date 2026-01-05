using CombatRoom;
using Managers;
using Player;
using ShopRoom;
using Targets;
using UnityEngine;

namespace Items
{
    public class IncreaseRadiusTargetItem : ShopItem
    {
        protected override void OnPurchased(PlayerCombatant player)
        {
            base.OnPurchased(player);
            
            TargetScaleModifier modifier = new TargetScaleModifier(shopItemData.itemModifier);
            TargetModifierManager.Instance.AddModifier(modifier);
        }
    }
}