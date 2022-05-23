using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Code.GameRooms
{
    public class RoomsList : MonoBehaviourPunCallbacks
    {
        [SerializeField] private JoinRoomButton _joinRoomButton;
        [SerializeField] private Transform _root;
        private Dictionary<string, RoomInfo> _cachedRooms;
        private Dictionary<string, GameObject> _rooms;

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRooms();
            AddRooms(roomList);
        }

        private void AddRooms(List<RoomInfo> roomList)
        {
            _cachedRooms = new Dictionary<string, RoomInfo>();
            _rooms = new Dictionary<string, GameObject>();

            foreach (RoomInfo room in roomList)
            {
                string roomName = room.Name;
                _cachedRooms.Add(roomName, room);
                var joinRoomButtonWidget = Instantiate(_joinRoomButton, _root);
                joinRoomButtonWidget.SetRoomName(room.Name);
                _rooms.Add(roomName, joinRoomButtonWidget.gameObject);
            }
        }

        private void ClearRooms()
        {
            foreach (GameObject entry in _rooms.Values)
                Destroy(entry.gameObject);

            _cachedRooms.Clear();
        }
    }
}
