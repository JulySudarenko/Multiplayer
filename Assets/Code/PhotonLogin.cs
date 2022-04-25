using Photon.Pun;
using UnityEngine;


namespace Code
{
    public class PhotonLogin : MonoBehaviourPunCallbacks
    {
        string gameVersion = "1";

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
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
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        
        public override void OnConnectedToMaster()
        {
           base.OnConnectedToMaster(); 
            Debug.Log($"Successeful connected to master! Region: {PhotonNetwork.CloudRegion}, ping: {PhotonNetwork.GetPing()}");
            //Debug.Log("OnConnectedToMaster() was called by PUN");
        }

    }
}
