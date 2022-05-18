using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GameRooms
{
    public class JoinRoomButton : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Text _roomName;
        [SerializeField] private Button _joinRoomButton;

        private void Awake()
        {
            _joinRoomButton.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _joinRoomButton.onClick.RemoveListener(OnClick);
        }

        public override void OnConnected()
        {
            _joinRoomButton.interactable = PhotonNetwork.Server == ServerConnection.MasterServer;
        }

        public void SetRoomName(string roomName)
        {
            _roomName.text = roomName;
        }

        private void OnClick()
        {
            if (PhotonNetwork.Server != ServerConnection.MasterServer)
                return;

            PhotonNetwork.JoinRoom(_roomName.text);
        }
    }
}
