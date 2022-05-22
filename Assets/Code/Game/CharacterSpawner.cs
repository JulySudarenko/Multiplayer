using Photon.Pun;
using UnityEngine;


namespace Code.Game
{
    internal class CharacterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;

        private void Start()
        {
            var number = PhotonNetwork.LocalPlayer.ActorNumber;
            PhotonNetwork.Instantiate(_playerPrefab.name, transform.GetChild(number).position, Quaternion.identity);
        }
    }
}
