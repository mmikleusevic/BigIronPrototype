using System;
using Cysharp.Threading.Tasks;
using Extensions;
using Managers;
using Player;
using ShopRoom;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject itemInfoPanel;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button leaveButton;
        [SerializeField] private ShopController shopController;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemInfoText;
        
        private void Awake()
        {
            itemInfoPanel.SetActive(false);
            buyButton.interactable = false;
        }

        private void OnEnable()
        {
            shopController.OnTryBuy += OnTryBuy;
            shopController.OnItemSelected += OnItemSelected;
            buyButton.onClick.AddListener(Buy);
            leaveButton.AddClickAsync(Leave);
        }

        private void OnDisable()
        {
            shopController.OnTryBuy -= OnTryBuy;
            shopController.OnItemSelected -= OnItemSelected;
            buyButton.onClick.RemoveListener(Buy);
            leaveButton.onClick.RemoveAllListeners();
        }


        private void OnTryBuy(ShopItem shopItem, bool hasBought)
        {
            itemInfoPanel.SetActive(true);
            
            itemNameText.gameObject.SetActive(false);
            
            if (hasBought)
            {
                itemInfoText.text = "Bought " + shopItem.ShopItemData.itemName;
            }
            else
            {
                itemInfoText.text = "Insufficient funds for: " + shopItem.ShopItemData.itemName;    
            }
        }
        
        private void OnItemSelected(ShopItem shopItem)
        {
            if (!shopItem)
            {
                itemInfoPanel.SetActive(false);
                buyButton.interactable = false;
                return;
            }
            
            itemInfoPanel.SetActive(true);
            itemNameText.gameObject.SetActive(true);
            buyButton.interactable = true;
            
            itemNameText.text = shopItem.ShopItemData.itemName;
            itemInfoText.text = shopItem.ShopItemData.itemDescription;
        }

        private void Buy()
        {
            buyButton.interactable = false;
            shopController.Buy();
        }
        
        private async UniTask Leave()
        {
            if (!LevelManager.Instance) return; 
            
            await LevelManager.Instance.UnloadSceneActivateGame(shopController.ShopAssetReference);
        }
    }
}