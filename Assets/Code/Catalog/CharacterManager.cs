using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Catalog
{
    public class CharacterManager
    {
        private const string GOLD = "GD";

        private readonly PlayerNamePanelView _enterNamePanel;
        private readonly Transform _charactersPanel;
        private readonly LineElementView _lineElement;

        private readonly Dictionary<string, StoreItem> _characterCatalog = new Dictionary<string, StoreItem>();
        private readonly Dictionary<string, CatalogItem> _catalog;
        private List<LineElementView> _elements = new List<LineElementView>();
        private StoreItem _storeItem;

        private string _characterName;

        public CharacterManager(PlayerNamePanelView enterNamePanel, Transform charactersPanel,
            LineElementView lineElement, Dictionary<string, CatalogItem> catalog)
        {
            _enterNamePanel = enterNamePanel;
            _charactersPanel = charactersPanel;
            _lineElement = lineElement;
            _catalog = catalog;

            _enterNamePanel.NameInput.onEndEdit.AddListener(SetCharacterName);
            _enterNamePanel.AcceptNameButton.onClick.AddListener(CreateNewCharacter);
            
            ShowAllUserCharacters();
            CreateCharacterCatalog();
        }

        public void SetCharacterName(string newName)
        {
            _characterName = newName;
        }

        private void CreateCharacterCatalog()
        {
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
            {
                CatalogVersion = "0.1",
                StoreId = "Characters_store"
            }, result =>
            {
                foreach (var storeItem in result.Store)
                {
                    _characterCatalog.Add(storeItem.ItemId, storeItem);
                }

                CreateAddCharactersButtons();
            }, Error);
        }

        private void ShowAllUserCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    Debug.Log($"Characters owned: + {result.Characters.Count}");
                    foreach (var character in result.Characters)
                    {
                        var characterLine = Object.Instantiate(_lineElement, _charactersPanel);
                        characterLine.gameObject.SetActive(true);
                        characterLine.TextUp.text = $"{character.CharacterName} {character.CharacterType}";
                        UpdateCharacterView(character.CharacterId, characterLine.TextDown);
                        _elements.Add(characterLine);
                    }
                }, Error);
        }

        private void CreateAddCharactersButtons()
        {
            foreach (var storeItem in _characterCatalog)
            {
                var newAddButton = Object.Instantiate(_lineElement, _charactersPanel);
                newAddButton.gameObject.SetActive(true);
                newAddButton.TextUp.text = _catalog[storeItem.Key].DisplayName;
                newAddButton.TextDown.text = $"{storeItem.Value.VirtualCurrencyPrices[GOLD].ToString()} {GOLD}";
                newAddButton.Button.onClick.AddListener(() => CreateCharacterPanel(storeItem.Value));
                _elements.Add(newAddButton);
            }
        }

        private void CreateCharacterPanel(StoreItem storeItem)
        {
            _storeItem = storeItem;
            _enterNamePanel.OpenClosePanel(true);
        }

        private void CreateNewCharacter()
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = _storeItem.ItemId,
                Price = (int) _storeItem.VirtualCurrencyPrices[GOLD],
                VirtualCurrency = GOLD
            }, result =>
            {
                CreateCharacterWithItemId(_storeItem.ItemId);
            }, Error);
        }

        private void CreateCharacterWithItemId(string itemId)
        {
            _enterNamePanel.AcceptNameButton.interactable = false;
            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
            {
                CharacterName = _characterName,
                ItemId = itemId
            }, result =>
            {
                Debug.Log($"Get character type: {result.CharacterType}");

                UpdateCharacterStatistics(result.CharacterId);
            }, Error);
        }

        private void UpdateCharacterStatistics(string characterId)
        {
            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
            {
                CharacterId = characterId,
                CharacterStatistics = new Dictionary<string, int>
                {
                    {"Level", 1},
                    {"Exp", 0},
                    {"Gold", 0},
                    {"Damage", 20},
                    {"Health", 100}
                }
            }, result =>
            {
                _enterNamePanel.OpenClosePanel(false);
                _enterNamePanel.AcceptNameButton.interactable = true;

                UpdateCharacterSlots(characterId);
            }, Error);
        }

        private void UpdateCharacterSlots(string characterId)
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    foreach (var character in result.Characters)
                    {
                        if (character.CharacterId == characterId)
                        {
                            var characterLine = Object.Instantiate(_lineElement, _charactersPanel);
                            characterLine.gameObject.SetActive(true);
                            characterLine.TextUp.text = $"{character.CharacterName} {character.CharacterType}";

                            UpdateCharacterView(character.CharacterId, characterLine.TextDown);
                            _elements.Add(characterLine);
                        }
                    }
                }, Error);
        }

        public void UpdateCharacterView(string characterId, TMP_Text text)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                {
                    CharacterId = characterId
                },
                result => { text.text = result.CharacterStatistics["Level"].ToString(); },
                Debug.LogError);
        }

        private void Error(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
            _enterNamePanel.AcceptNameButton.interactable = true;
        }

        public void OnDestroy()
        {
            _enterNamePanel.NameInput.onEndEdit.RemoveListener(SetCharacterName);
            _enterNamePanel.AcceptNameButton.onClick.RemoveListener(CreateNewCharacter);
            foreach (var element in _elements)
            {
                element.Button.onClick.RemoveAllListeners();
            }
        }
    }
}
