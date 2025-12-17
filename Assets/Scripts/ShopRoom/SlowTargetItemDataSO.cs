using UnityEngine;

namespace ShopRoom
{
    [CreateAssetMenu(menuName = "ShopItem/SlowTargetItemData")]
    public class SlowTargetItemDataSO : ShopItemDataSO
    {
        protected override void OnValidate()
        {
            itemDescription = $"Slows target speed by {Mathf.RoundToInt(itemModifier * 100) - 100}%";
        }
    }
}