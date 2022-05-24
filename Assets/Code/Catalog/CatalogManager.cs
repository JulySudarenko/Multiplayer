using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Catalog
{
    public class CatalogManager
    {
        private readonly ShopLobby _shopLobby;
        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
        private readonly CharacterLobby _characterLobby;

        public CatalogManager(Transform shopPanel, Transform inventoryPanel, Transform characterPanel,
            PlayerNamePanelView enterNamePanel, TextElementView gold, TextElementView experience,
            LineElementView lineElement)
        {
            var inventoryLobby =
                new InventoryLobby(inventoryPanel, gold, experience, lineElement, _catalog);
            
            _shopLobby = new ShopLobby(shopPanel, _catalog, lineElement, inventoryLobby);
            _characterLobby = new CharacterLobby(enterNamePanel, characterPanel, lineElement, _catalog, inventoryLobby);
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);
            inventoryLobby.UpdateInventory();
            inventoryLobby.UpdateCurrency();
        }

        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

        private void OnGetCatalogSuccess(GetCatalogItemsResult result)
        {
            HandleCatalog(result.Catalog);
            _shopLobby.SetStoreItems();
            _characterLobby.LoadCharacters();
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
