using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Code
{
    public class PhotonLogin : MonoBehaviourPunCallbacks
    {
        [SerializeField] private ButtonColorWidget _connectButton;
        [SerializeField] private ButtonColorWidget _disconnectButton;

        string gameVersion = "1";

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            _connectButton.ChangeColor(ConnectionState.Default, "Connect");
            _disconnectButton.ChangeColor(ConnectionState.Default, "Disconnect");

            _connectButton.ColoringButton.onClick.AddListener(OnConnectedClick);
            _disconnectButton.ColoringButton.onClick.AddListener(OnDisconnectedClick);
        }

        private void OnDisconnectedClick()
        {
            _connectButton.ChangeColor(ConnectionState.Default, "Disconnecting process...");
            if (PhotonNetwork.IsConnected)
            {
                _disconnectButton.ChangeColor(ConnectionState.Waiting, "Disconnecting process...");
                PhotonNetwork.Disconnect();
            }
        }

        private void OnConnectedClick()
        {
            _disconnectButton.ChangeColor(ConnectionState.Default, "Waiting for connection");
            Connect();
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                _connectButton.ChangeColor(ConnectionState.Waiting, "Waiting for connection");

                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            _connectButton.ChangeColor(ConnectionState.Success,
                $"Successful connected to Photon! Region: {PhotonNetwork.CloudRegion}, ping: {PhotonNetwork.GetPing()}");
             
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);

            _disconnectButton.ChangeColor(ConnectionState.Success, "Disconnected successful");
        }

        private void OnDestroy()
        {
            _connectButton.ColoringButton.onClick.RemoveAllListeners();
            _disconnectButton.ColoringButton.onClick.RemoveAllListeners();
        }
    }
}
