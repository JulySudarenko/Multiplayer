using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Code.Catalog
{
    public class InventoryLobby
    {
        private readonly TextElementView _inventoryItem;
        private readonly Transform _inventory;
        private readonly TextElementView _gold;
        private readonly TextElementView _experience;
        private readonly List<TextElementView> _itemList = new List<TextElementView>();
        private CatalogItem _item;

        public InventoryLobby(TextElementView inventoryItem, Transform inventory, TextElementView gold,
            TextElementView experience)
        {
            _inventoryItem = inventoryItem;
            _inventory = inventory;
            _gold = gold;
            _experience = experience;
        }

        public void UpdateInventory()
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

                    foreach (var t in _itemList)
                    {
                        Object.Destroy(t.gameObject);
                    }
                    _itemList.Clear();
                    
                    foreach (var inv in success.Inventory)
                    {
                        var item = Object.Instantiate(_inventoryItem, _inventory);
                        item.ShowName(inv.DisplayName);
                        item.gameObject.SetActive(true);
                        _itemList.Add(item);
                    }
                },
                error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
        }

    }
}
