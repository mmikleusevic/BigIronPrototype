using UnityEngine;

namespace ShopRoom
{
    [CreateAssetMenu(menuName = "ShopItem/IncreaseRadiusTargetItemData")]
    public class IncreaseRadiusTargetDataSO : ShopItemDataSO
    {
        public override string ItemDescription =>
            $"Increases target radius by {Mathf.RoundToInt(itemModifier * 100) - 100}%";
    }
}