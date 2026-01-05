using System;
using Managers;
using Player;
using TMPro;
using UnityEngine;

namespace ShopRoom
{
    public abstract class ShopItem : MonoBehaviour
    {
        [SerializeField] protected ShopItemDataSO shopItemData;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject highlight;
        public ShopItemDataSO ShopItemData => shopItemData;
        public string ItemId => shopItemData.itemName;

        private void Awake()
        {
            SetSelection(false);
        }

        private void Start()
        {
            nameText.text = shopItemData.itemName;
        }

        public void Select()
        {
            SetSelection(true);
        }

        public void Deselect()
        {
            SetSelection(false);
        }

        private bool CanBuy(PlayerCombatant player)
        {
            return player.Gold.GoldAmount >= shopItemData.price;
        }

        public bool TryBuy(PlayerCombatant player)
        {
            if (!CanBuy(player)) return false;

            player.Gold.LoseGoldAmount(shopItemData.price);
            OnPurchased(player);
            return true;
        }

        private void SetSelection(bool value)
        {
            highlight.SetActive(value);
            nameText.gameObject.SetActive(value);
        }

        protected virtual void OnPurchased(PlayerCombatant player)
        {
            player.OwnedItemIds.Add(shopItemData.itemName);
        }
    }
}