using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Catalog
{
    public class CatalogManager
    {
        private readonly Shop _shop;

        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();


        public CatalogManager(TextElementView gold, TextElementView experience, Transform shop,
            ItemStoreElementView item, Transform inventory, TextElementView inventoryItem)
        {
            var inventory1 = new Inventory(inventoryItem, inventory, gold, experience);
            _shop = new Shop(shop, item, inventory1, _catalog);
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
            inventory1.UpdateInventory();
        }

        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

        private void OnGetCatalogSuccess(GetCatalogItemsResult result)
        {
            HandleCatalog(result.Catalog);
            _shop.SetStoreItems();
            Debug.Log($"Catalog was loaded successfully!");
        }

        private void HandleCatalog(List<CatalogItem> catalog)
        {
            foreach (var item in catalog)
            {
                _catalog.Add(item.ItemId, item);
            }
        }
    }
}
