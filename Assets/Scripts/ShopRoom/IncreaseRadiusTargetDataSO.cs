using UnityEngine;

namespace ShopRoom
{
    [CreateAssetMenu(menuName = "ShopItem/IncreaseRadiusTargetItemData")]
    public class IncreaseRadiusTargetDataSO : ShopItemDataSO
    {
        protected override void OnValidate()
        {
            itemDescription = $"Increases target radius by {Mathf.RoundToInt(itemModifier * 100) - 100}%";
        }
    }
}