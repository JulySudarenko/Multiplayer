using System;
using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Catalog
{
    public class InventoryLobby : IDisposable
    {
        private readonly Dictionary<string, CatalogItem> _catalog;
        private readonly List<LineElementView> _lineElements = new List<LineElementView>();
        private readonly Transform _inventoryPanel;
        private readonly TextElementView _gold;
        private readonly TextElementView _experience;
        private readonly LineElementView _lineElementView;

        public InventoryLobby(Transform inventoryPanel, TextElementView gold, TextElementView experience,
            LineElementView lineElementView, Dictionary<string, CatalogItem> catalog)
        {
            _inventoryPanel = inventoryPanel;
            _gold = gold;
            _experience = experience;
            _lineElementView = lineElementView;
            _catalog = catalog;
        }

        public void UpdateCurrency()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                success =>
                {
                    foreach (KeyValuePair<string, int> currency in success.VirtualCurrency)
                    {
                        if (currency.Key == "GD")
                            _gold.ShowCurrency(currency.Key, currency.Value.ToString());
                        else if (currency.Key == "EX")
                            _experience.ShowCurrency(currency.Key, currency.Value.ToString());
                        else
                        {
                            Debug.Log(currency);
                        }
                    }
                },
                error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
        }
        
        public void UpdateInventory()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                success =>
                {
                    foreach (var item in _lineElements)
                    {
                        Object.Destroy(item.gameObject);
                    }

                    _lineElements.Clear();

                    foreach (var invItem in success.Inventory)
                    {
                        var item = Object.Instantiate(_lineElementView, _inventoryPanel);
                        item.TextUp.text = invItem.DisplayName;
                        item.gameObject.SetActive(true);
                        _lineElements.Add(item);
                        //item.Button.onClick.AddListener(() => UseInventoryItem(_catalog[invItem.ItemId]));
                    }
                },
                error => { Debug.LogError($"Get User Inventory Failed: {error}"); });
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
