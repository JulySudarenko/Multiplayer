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
        [SerializeField] private GameObject _roomsJoinedPanel;
        [SerializeField] private GameObject _playerList;
        [SerializeField] private PlayerNamePanelView _playerNamePanelView;
        [SerializeField] private TextElementView _element;
        [SerializeField] private InputField _roomNameInputField;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _changePlayerNameButton;
        [SerializeField] private TMP_Text _playerNameText;

        private PlayerNamePanel _playerNamePanel;
        private string _roomName;
        
        private Dictionary<string, TextElementView> _roomPlayers = new Dictionary<string, TextElementView>();
        
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _roomNameInputField.onEndEdit.AddListener(UpdateRoomName);
            _createButton.onClick.AddListener(OnCreateRoomButtonClicked);
            _startButton.onClick.AddListener(OnStartGameButtonClicked);
            _changePlayerNameButton.onClick.AddListener(OpenChangePlayerNamePanel);
            _startButton.gameObject.SetActive(false);
            _roomsJoinedPanel.SetActive(true);
            
            _playerNamePanel = new PlayerNamePanel(_playerNamePanelView, _playerNameText);
            _playerNamePanel.ActivatePanel();
        }

        private void Start()
        {
            Connect();
        }

        private void Connect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private void OpenChangePlayerNamePanel()
        {
             _playerNamePanel.ActivatePanel();
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
            PhotonNetwork.CreateRoom(_roomName);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"Room creation failed {message}");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            _roomsJoinedPanel.SetActive(false);
            _startButton.gameObject.SetActive(true);

            foreach (var p in PhotonNetwork.PlayerList)
            {
                var newElement = Instantiate(_element, _element.transform.parent);
                newElement.gameObject.SetActive(true);
                newElement.ShowRoom(p);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            if (!_roomPlayers.ContainsKey(newPlayer.NickName))
            {
                var playerItem = Instantiate(_element, _element.transform.parent);
                playerItem.ShowName(newPlayer.NickName);
                _roomPlayers.Add(newPlayer.NickName, playerItem);
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            var nickName = otherPlayer.NickName;
            if (_roomPlayers.ContainsKey(nickName))
            {
                Destroy(_roomPlayers[nickName].gameObject);
                _roomPlayers.Remove(nickName);
            }
        }
        
        private void OnStartGameButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel("GameForest");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _createButton.gameObject.SetActive(false);
            _roomNameInputField.gameObject.SetActive(false);
            _roomsJoinedPanel.gameObject.SetActive(false);
            _playerList.SetActive(true);
            foreach (var p in roomList)
            {
                var newElement = Instantiate(_element, _element.transform.parent);
                newElement.gameObject.SetActive(true);
                newElement.ShowName(p.Name);
            }
        }
        
        public void OnDestroy()
        {
            _roomNameInputField.onEndEdit.RemoveListener(UpdateRoomName);
            _createButton.onClick.RemoveListener(OnCreateRoomButtonClicked);
            _startButton.onClick.RemoveListener(OnStartGameButtonClicked);
            _changePlayerNameButton.onClick.RemoveListener(OpenChangePlayerNamePanel);
        }
    }
}
