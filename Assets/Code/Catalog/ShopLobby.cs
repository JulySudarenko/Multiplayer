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
        private readonly Dictionary<string, CatalogItem> _catalog;
        private readonly List<LineElementView> _lineElements = new List<LineElementView>();
        private readonly Transform _shopPanel;
        private readonly LineElementView _lineElementView;
        private readonly InventoryLobby _inventoryLobby;

        public ShopLobby(Transform shopPanel, Dictionary<string, CatalogItem> catalog, LineElementView lineElementView,
            InventoryLobby inventoryLobby)
        {
            _shopPanel = shopPanel;
            _inventoryLobby = inventoryLobby;
            _catalog = catalog;
            _lineElementView = lineElementView;
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
                    var newItem = Object.Instantiate(_lineElementView, _shopPanel);
                    newItem.gameObject.SetActive(true);
                    newItem.TextUp.text = _catalog[item.ItemId].DisplayName;
                    newItem.TextDown.text = $"{item.VirtualCurrencyPrices["GD"]} GD";
                    newItem.Button.onClick.AddListener(() => BuyItem(_catalog[item.ItemId]));
                    _lineElements.Add(newItem);
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
                    success => 
                    { 
                        _inventoryLobby.UpdateCurrency();
                        _inventoryLobby.UpdateInventory(); 
                    }, error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _lineElements.Count; i++)
            {
                _lineElements[i].Button.onClick.RemoveAllListeners();
            }
        }
    }
}
