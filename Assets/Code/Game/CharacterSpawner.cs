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
            Debug.Log(number);
            // Debug.Log(transform.GetChild(Mathf.Abs(number)).position);
            // Почему присваивает -1, если сразу загрузить эту сцену?
            PhotonNetwork.Instantiate(_playerPrefab.name, transform.GetChild(Mathf.Abs(number)).position, Quaternion.identity);
        }
    }
}
