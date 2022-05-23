using System.Collections.Generic;
using Code.View;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Catalog
{
    public class CharacterManager : MonoBehaviourPunCallbacks
    {
        private const string GOLD = "GD";

        [SerializeField] private PlayerNamePanelView _enterNamePanel;
        private string _characterName;
        public static string _playCharacterName;

        [Space(20)] [SerializeField] private Button[] _addCharacters;
        [SerializeField] private LineElementView[] _useCharacters;

        private void Start()
        {
            UpdateCharacterSlots();
            _enterNamePanel.NameInput.onEndEdit.AddListener(SetCharacterName);
             foreach (var character in _addCharacters)
             {
                 character.gameObject.SetActive(false);
                 character.onClick.AddListener(() => _enterNamePanel.OpenClosePanel(true));
             }
            
             for (int i = 0; i < _useCharacters.Length; i++)
             {
                 var temp = i;
                 _useCharacters[i].gameObject.SetActive(false);
                 _useCharacters[i].Button.onClick.AddListener(() =>
                 {
                     _playCharacterName = _useCharacters[temp].TextUp.text;
                 });
             }
            
             _enterNamePanel.AcceptNameButton.onClick.AddListener(GetCharacterTokens);
        }

        public void SetCharacterName(string newName)
        {
            _characterName = newName;
        }
        
        private void GetCharacterTokens()
        {
            _enterNamePanel.AcceptNameButton.interactable = false;
            PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
            {
                CatalogVersion = "0.1",
                StoreId = "Characters_store"
            }, result =>
            {
                CreateNewCharacter(result.Store[0]);

            }, Error);
        }
        
         private void CreateNewCharacter(StoreItem storeItem)
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = storeItem.ItemId,
                Price = (int) storeItem.VirtualCurrencyPrices[GOLD],
                VirtualCurrency = GOLD
            }, result =>
            {
                foreach (var item in result.Items)
                {
                    CreateCharacterWithItemId(item.ItemId);
                    return;
                }
            }, Error);
        }
        
        private void CreateCharacterWithItemId(string itemId)
        {
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
                    {"Gold", 0}
                }
            }, result =>
            {
                _enterNamePanel.OpenClosePanel(false);
                _enterNamePanel.AcceptNameButton.interactable = true;
        
                UpdateCharacterSlots();
            }, Error);
        }

        private void UpdateCharacterSlots()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    Debug.Log($"Count Characters: {result.Characters.Count}");

                    foreach (var character in _addCharacters)
                        character.gameObject.SetActive(true);
                    foreach (var character in _useCharacters)
                        character.gameObject.SetActive(false);

                    for (int i = 0; i < result.Characters.Count; i++)
                    {
                        _addCharacters[i].gameObject.SetActive(false);
                        _useCharacters[i].gameObject.SetActive(true);

                        _useCharacters[i].TextUp.text = result.Characters[i].CharacterName;
                        UpdateCharacterView(result.Characters[i].CharacterId, i);
                    }
                }, Debug.LogError);
        }
        
        public void UpdateCharacterView(string characterId, int numberSlots)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            {
                CharacterId = characterId
            }, result =>
            {
                _useCharacters[numberSlots].TextDown.text = result.CharacterStatistics["Level"].ToString();
            }, Debug.LogError);
        }
        
        private void Error(PlayFabError error)
        {
            Debug.LogError(error.GenerateErrorReport());
            _enterNamePanel.AcceptNameButton.interactable = true;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            UpdateCharacterSlots();
        }

        private void OnDestroy()
        {
            _enterNamePanel.NameInput.onEndEdit.RemoveListener(SetCharacterName);
        }
    }
}
