using Code.View;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Code.GameRooms
{
    internal class PlayerNamePanel
    {
        private readonly PlayerNamePanelView _namePanelView;
        private readonly TMP_Text _playerNameText;
        
        private string _playerName;

        public PlayerNamePanel(PlayerNamePanelView namePanelView, TMP_Text playerNameText)
        {
            _namePanelView = namePanelView;
            _playerNameText = playerNameText;
            _namePanelView.NameInput.onValueChanged.AddListener(SetName);
            _namePanelView.AcceptNameButton.onClick.AddListener(SavePlayerName);
        }

        private void SetName(string newName)
        {
            _playerName = newName;
            _playerNameText.text = _playerName;
        }

        public void ActivatePanel()
        {
            _namePanelView.OpenClosePanel(true);
        }

        private void SavePlayerName()
        {
            _namePanelView.AcceptNameButton.interactable = false;
            PlayFabClientAPI.UpdateUserTitleDisplayName(
                new UpdateUserTitleDisplayNameRequest {DisplayName = _playerName},
                result =>
                {
                    PhotonNetwork.NickName = result.DisplayName;
                    _namePanelView.OpenClosePanel(false);
                }, error =>
                {
                    Debug.LogError(error);
                    _namePanelView.AcceptNameButton.interactable = true;
                });
        }
    }
}
