using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GameRooms
{
    public class SetRoomOpenedButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private bool _isRoomOpen;

        private void Awake()
        {
            if (PhotonNetwork.CurrentRoom == null && _button.interactable)
            {
                _button.interactable = false;
            }
            else if (PhotonNetwork.CurrentRoom != null && !_button.interactable)
                _button.interactable = true;
        
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.IsOpen != _isRoomOpen)
                _isRoomOpen = PhotonNetwork.CurrentRoom.IsOpen;

            _button.onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            if (PhotonNetwork.CurrentRoom == null)
                return;

            _isRoomOpen = !_isRoomOpen;
            PhotonNetwork.CurrentRoom.IsOpen = _isRoomOpen;
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
