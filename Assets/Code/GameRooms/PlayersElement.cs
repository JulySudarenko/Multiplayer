using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Code.GameRooms
{
    public class PlayersElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomName;

        public void SetRoom(Player room)
        {
            _roomName.text = room.UserId;
        }
    }
}
