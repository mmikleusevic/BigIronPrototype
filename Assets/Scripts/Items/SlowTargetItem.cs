using System.Collections;
using CombatRoom;
using Managers;
using Player;
using ShopRoom;
using Targets;
using UnityEngine;

namespace Items
{
    public class SlowTargetsItem : ShopItem
    {
        protected override void OnPurchased(PlayerCombatant player)
        {
            base.OnPurchased(player);
            
            TargetSpeedModifier modifier = new TargetSpeedModifier(shopItemData.itemModifier);
            TargetModifierManager.Instance.AddModifier(modifier);
        }
    }
}