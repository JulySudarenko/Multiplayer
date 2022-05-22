using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GameRooms
{
    public class PrivateRoomButton : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Text _roomName;
        [SerializeField] private Button _joinRoomButton;
        [SerializeField] private Button _getRoomNameButton;
        private bool _isPrivateRoomIdCreated;

        private void Awake()
        {
            _joinRoomButton.onClick.AddListener(OnJoinRoomClicked);
            _getRoomNameButton.onClick.AddListener(OnGetRoomNameClicked);
        }

        public override void OnConnected()
        {
            if (PhotonNetwork.Server != ServerConnection.MasterServer)
            {
                _joinRoomButton.interactable = false;
                _getRoomNameButton.interactable = false;
            }
            else
            {
                _joinRoomButton.interactable = true;
                _getRoomNameButton.interactable = true;
            }
        }
       
        private void OnJoinRoomClicked()
        {
            if (!_isPrivateRoomIdCreated)
                return;

            var roomOptions = new RoomOptions
            {
                IsVisible = false
            };

            PhotonNetwork.JoinOrCreateRoom(_roomName.text, roomOptions, TypedLobby.Default);
        }

        private void OnGetRoomNameClicked()
        {
            var guid = Guid.NewGuid().ToString();
            _roomName.text = guid;
            _isPrivateRoomIdCreated = true;
        }
        
        private void OnDestroy()
        {
            _joinRoomButton.onClick.RemoveListener(OnJoinRoomClicked);
            _getRoomNameButton.onClick.RemoveListener(OnGetRoomNameClicked);
        }
    }
}
