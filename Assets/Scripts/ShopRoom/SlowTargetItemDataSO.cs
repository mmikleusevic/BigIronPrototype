using UnityEngine;

namespace ShopRoom
{
    [CreateAssetMenu(menuName = "ShopItem/SlowTargetItemData")]
    public class SlowTargetItemDataSO : ShopItemDataSO
    {
        public override string ItemDescription =>
            $"Slows target speed by {Mathf.RoundToInt(itemModifier * 100) - 100}%";
    }
}