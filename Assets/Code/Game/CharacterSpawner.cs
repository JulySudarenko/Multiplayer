using Photon.Pun;
using UnityEngine;


namespace Code.Game
{
     internal class CharacterSpawner
     {
         private readonly Transform _spawnPlaces;

         public CharacterSpawner(Transform spawnPlaces)

         {
             _spawnPlaces = spawnPlaces;
         }
        
        public void SpawnSelectedCharacter(GameObject character)
        {
            var number = PhotonNetwork.LocalPlayer.ActorNumber;
            PhotonNetwork.Instantiate(character.name, _spawnPlaces.GetChild(Mathf.Abs(number)).position, Quaternion.identity);
        }
    }
}
