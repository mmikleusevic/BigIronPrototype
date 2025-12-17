using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace ShopRoom
{
    public class ShopController : MonoBehaviour
    {
        public event Action<ShopItem, bool> OnTryBuy;
        public event Action<ShopItem> OnItemSelected;
        
        [field: SerializeField] public AssetReference ShopAssetReference { get; private set; }

        [SerializeField] private ShopInputs shopInputs;
        [SerializeField] private ShopItem[] shopItemPrefabs;
        [SerializeField] private Transform[] itemTransforms;
        
        private readonly List<ShopItem> shopItems = new List<ShopItem>();
        private int selectedItemIndex = -1;
        
        private void Start()
        {
            SpawnItems();
        }

        private void OnEnable()
        {
            shopInputs.OnMove += MoveSelectedItem;
            shopInputs.OnConfirm += Buy;
            shopInputs.OnLeave += Leave;
        }

        private void OnDisable()
        {
            shopInputs.OnMove -= MoveSelectedItem;
            shopInputs.OnConfirm -= Buy;
            shopInputs.OnLeave -= Leave;
        }

        private void SpawnItems()
        {
            int count = Mathf.Min(shopItemPrefabs.Length, itemTransforms.Length);
            
            PlayerCombatant player = GameManager.Instance.PlayerCombatant;
            if (!player) return;
            
            List<ShopItem> shopItemPool = new List<ShopItem>();
            foreach (ShopItem prefab in shopItemPrefabs)
            {
                if (!player.OwnedItemIds.Contains(prefab.ItemId))
                {
                    shopItemPool.Add(prefab);
                }
            }

            if (shopItemPool.Count == 0) return;
            
            for (int i = 0; i < count; i++)
            {
                if (shopItemPool.Count == 0) break;

                int index = Random.Range(0, shopItemPool.Count);
                ShopItem randomItemPrefab = shopItemPool[index];
                shopItemPool.RemoveAt(index);
                
                Transform spawnPoint = itemTransforms[i];
                ShopItem shopItem = Instantiate(randomItemPrefab, spawnPoint.position, spawnPoint.rotation);
                shopItems.Add(shopItem);
            }

            MoveSelectedItem(Vector2.one);
        }

        private void MoveSelectedItem(Vector2 move)
        {
            ShopItem selectedItem = null;
            
            if (shopItems.Count > 0)
            {
                if (selectedItemIndex > -1 && selectedItemIndex < shopItems.Count) selectedItem = shopItems[selectedItemIndex];
                if (selectedItem) selectedItem.Deselect();
            
                if (move.x > 0.5f) selectedItemIndex++;
                else if (move.x < -0.5f) selectedItemIndex--;
                
                selectedItemIndex = (selectedItemIndex + shopItems.Count) % shopItems.Count;
                
                selectedItem = shopItems[selectedItemIndex];
            
                if (!selectedItem) return;
            
                selectedItem.Select();
            }
            else
            {
                selectedItemIndex = -1;
            }
            
            OnItemSelected?.Invoke(selectedItem);
        }

        public void Buy()
        {
            if (selectedItemIndex < 0) return;
            
            PlayerCombatant playerCombatant = GameManager.Instance.PlayerCombatant;
            if (!playerCombatant) return;
            
            ShopItem shopItem = shopItems[selectedItemIndex];
            bool hasBought = shopItem.TryBuy(playerCombatant);

            OnTryBuy?.Invoke(shopItem, hasBought);

            if (!hasBought) return;
            
            shopItem.Deselect();
            shopItems.Remove(shopItem);
            Destroy(shopItem.gameObject);
            
            MoveSelectedItem(Vector2.one);
        }
        
        
        private void Leave()
        {
            _ = LeaveAsync();
        }

        private async UniTask LeaveAsync()
        {
            if (!LevelManager.Instance) return;

            await LevelManager.Instance.UnloadSceneActivateGame(ShopAssetReference);   
        }
    }
}