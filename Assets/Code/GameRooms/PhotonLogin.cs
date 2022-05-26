using System.Collections.Generic;
using Code.View;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Code.GameRooms
{
    public class PhotonLogin : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform _playerList;
        [SerializeField] private GameObject _createRoomPanel;
        [SerializeField] private TextElementView _element;
        [SerializeField] private PlayerNamePanelView _playerNamePanelView;

        [SerializeField] private InputField _roomNameInputField;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _joinButton;
        [SerializeField] private TMP_Text _playerNameText;

        private PlayerNamePanel _playerNamePanel;
        private string _roomName;
        
    
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            
            _roomNameInputField.onEndEdit.AddListener(UpdateRoomName);
            _createButton.onClick.AddListener(OnCreateRoomButtonClicked);
            _startButton.onClick.AddListener(OnStartGameButtonClicked);
            _joinButton.onClick.AddListener(OnJoinRandomRoomClicked);
            
            _startButton.gameObject.SetActive(false);
            _createRoomPanel.SetActive(true);

            _playerNamePanel = new PlayerNamePanel(_playerNamePanelView, _playerNameText);
            _playerNamePanel.ActivatePanel();
        }

        private void Start()
        {
            Connect();
        }

        public void Connect()
        {
            if (!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinRandomRoom();
        }

        private void UpdateRoomName(string roomName)
        {
            _roomName = roomName;
        }

        private void OnCreateRoomButtonClicked()
        {
            //     PhotonNetwork.CreateRoom(_roomName);
            PhotonNetwork.JoinRandomOrCreateRoom();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"Room creation failed {message}");
        }

        public override void OnJoinedRoom()
        {
            _createRoomPanel.SetActive(false);
            _startButton.gameObject.SetActive(true);
            
            foreach (var p in PhotonNetwork.PlayerList)
            {
                var newElement = Instantiate(_element, _playerList);
                newElement.gameObject.SetActive(true);
                newElement.ShowRoom(p);
            }
        }


        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _createRoomPanel.SetActive(false);
            foreach (var p in roomList)
            {
                var newElement = Instantiate(_element, _playerList);
                newElement.gameObject.SetActive(true);
                newElement.ShowName(p.Name);
            }
        }
        
        private void OnStartGameButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        
            PhotonNetwork.LoadLevel("GameForest");
        }

        private void OnJoinRandomRoomClicked()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        
        public void OnDestroy()
        {
            _roomNameInputField.onEndEdit.RemoveListener(UpdateRoomName);
            _createButton.onClick.RemoveListener(OnCreateRoomButtonClicked);
            _startButton.onClick.RemoveListener(OnStartGameButtonClicked);
            _joinButton.onClick.RemoveListener(OnJoinRandomRoomClicked);
        }



    }
}
