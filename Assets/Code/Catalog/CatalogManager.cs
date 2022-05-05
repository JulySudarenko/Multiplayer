using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Catalog
{
    public class CatalogManager : MonoBehaviour
    {
        private CurrencyStoreElement _gold;
        private CurrencyStoreElement _experience;
        private Transform _content;
        private ItemStoreElement _item;

        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
        private List<ItemStoreElement> _itemStoreElements;

        public void CreatCatalog(CurrencyStoreElement gold, CurrencyStoreElement experience, Transform content,
            ItemStoreElement item)
        {
            _gold = gold;
            _experience = experience;
            _content = content;
            _item = item;
            _itemStoreElements = new List<ItemStoreElement>();

            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
            UpdateCurrencyElement();
        }

        private void UpdateCurrencyElement()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                success =>
                {
                    foreach (KeyValuePair<string, int> kv in success.VirtualCurrency)
                    {
                        if (kv.Key == "GD")
                            _gold.ShowCurrency(kv.Key, kv.Value.ToString());
                        else if (kv.Key == "EX")
                            _experience.ShowCurrency(kv.Key, kv.Value.ToString());
                        else
                        {
                            Debug.Log(kv);
                        }
                    }
                },
                error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
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
                    success => { UpdateCurrencyElement(); },
                    error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
            }
        }

        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

        private void OnGetCatalogSuccess(GetCatalogItemsResult result)
        {
            HandleCatalog(result.Catalog);
            Debug.Log($"Catalog was loaded successfully!");
        }

        private void HandleCatalog(List<CatalogItem> catalog)
        {
            foreach (var item in catalog)
            {
                _catalog.Add(item.ItemId, item);
                if (item.VirtualCurrencyPrices.Count > 0)
                {
                    var newItem = Instantiate(_item, _content);
                    newItem.gameObject.SetActive(true);
                    var cost = (int) item.VirtualCurrencyPrices["GD"];
                    newItem.ShowItem(item.DisplayName, cost.ToString());

                    newItem.BuyButton.onClick.AddListener(() => BuyItem(item));
                    _itemStoreElements.Add(newItem);
                }
            }
        }

        public void OnDestroy()
        {
            for (int i = 0; i < _itemStoreElements.Count; i++)
            {
                _itemStoreElements[i].BuyButton.onClick.RemoveAllListeners();
            }
        }
    }
}
