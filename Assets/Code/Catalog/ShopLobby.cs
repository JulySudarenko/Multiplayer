using System;
using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Catalog
{
    public class ShopLobby : IDisposable
    {
        private readonly Transform _shop;
        private readonly ItemStoreElementView _item;
        private readonly List<ItemStoreElementView> _itemStoreElements;
        private readonly Dictionary<string, CatalogItem> _catalog;
        private readonly InventoryLobby _inventoryLobby;

        public ShopLobby(Transform shop, ItemStoreElementView item, InventoryLobby inventoryLobby,
            Dictionary<string, CatalogItem> catalog)
        {
            _shop = shop;
            _item = item;
            _inventoryLobby = inventoryLobby;
            _catalog = catalog;
            _itemStoreElements = new List<ItemStoreElementView>();
        }

        public void SetStoreItems()
        {
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
            {
                CatalogVersion = "0.1",
                StoreId = "ls1"
            }, result =>
            {
                foreach (var item in result.Store)
                {
                    var newItem = Object.Instantiate(_item, _shop);
                    newItem.gameObject.SetActive(true);
                    
                    var name = _catalog[item.ItemId].DisplayName;
                    var cost = (int) item.VirtualCurrencyPrices["GD"];
                    newItem.ShowItem(name, cost.ToString());

                    newItem.BuyButton.onClick.AddListener(() => BuyItem(_catalog[item.ItemId]));
                    _itemStoreElements.Add(newItem);
                }
            }, Debug.LogError);
        }

        private void BuyItem(CatalogItem catalogItem)
        {
            if (catalogItem.VirtualCurrencyPrices.ContainsKey("GD"))
            {
                PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
                    {
                        CatalogVersion = catalogItem.CatalogVersion,
                        ItemId = catalogItem.ItemId,
                        Price = (int) catalogItem.VirtualCurrencyPrices["GD"],
                        VirtualCurrency = "GD"
                    },
                    success => { _inventoryLobby.UpdateInventory(); }, // UpdateInventory(); },
                    error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _itemStoreElements.Count; i++)
            {
                _itemStoreElements[i].BuyButton.onClick.RemoveAllListeners();
            }
        }
    }
}
