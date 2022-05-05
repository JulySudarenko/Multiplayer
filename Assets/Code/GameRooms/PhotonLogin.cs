using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GameRooms
{
    public class PhotonLogin : MonoBehaviourPunCallbacks
    {
        private string _roomName;

        [SerializeField] private PlayersElement _element;
        [SerializeField] private InputField _roomNameInputField;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _startButton;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _roomNameInputField.onEndEdit.AddListener(UpdateRoomName);
            _createButton.onClick.AddListener(OnCreateRoomButtonClicked);
            _startButton.onClick.AddListener(OnStartGameButtonClicked);
            _startButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            Connect();
        }

        private void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
                Debug.Log($"Connected!!! {PhotonNetwork.AuthValues}");
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public void UpdateRoomName(string roomName)
        {
            _roomName = roomName;
        }

        public void OnCreateRoomButtonClicked()
        {
            PhotonNetwork.CreateRoom(_roomName);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"Room creation failed {message}");
        }

        public override void OnJoinedRoom()
        {
            _startButton.gameObject.SetActive(true);

            foreach (var p in PhotonNetwork.PlayerList)
            {
                var newElement = Instantiate(_element, _element.transform.parent);
                newElement.gameObject.SetActive(true);
                newElement.SetRoom(p);
            }
        }

        public void OnStartGameButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            Debug.Log("GameStart");
            //PhotonNetwork.LoadLevel("TestGameLevel");
        }

        public void OnDestroy()
        {
            _roomNameInputField.onEndEdit.RemoveListener(UpdateRoomName);
            _createButton.onClick.RemoveListener(OnCreateRoomButtonClicked);
            _startButton.onClick.RemoveListener(OnStartGameButtonClicked);
        }
    }
}
