using System;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


namespace Code.Game
{
    public class CharacterManager : MonoBehaviourPun, IPunObservable
    {
        public float Health => _health;

        [SerializeField] private GameObject _healthView;
        [SerializeField] private GameObject _magicalRay;

        private float _health;
        private bool _isFiring;

        private void Awake()
        {
            if (_magicalRay == null)
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> magical ray Reference.", this);
            }
            else
            {
                _magicalRay.SetActive(false);
            }

            GetUserData();
        }

        private void Start()
        {
            CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();
            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("Missing CameraWork Component on playerPrefab.", this);
            }
            
            if (_healthView != null)
            {
                GameObject _uiGo = Instantiate(_healthView);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }
        
        private void GetUserData()
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
            {
                Debug.Log("Got user data:");
                if (result.Data == null || !result.Data.ContainsKey("Health"))
                    Debug.Log("No params health");
                else
                {
                    _health = Convert.ToSingle(result.Data["Health"].Value);
                    Debug.Log("Health: " + result.Data["Health"].Value);
                }
            }, error =>
            {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
            }
        }

        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Attack"))
            {
                if (!_isFiring)
                {
                    _isFiring = true;
                    _magicalRay.SetActive(true);
                }
            }

            if (Input.GetButtonUp("Attack"))
            {
                if (_isFiring)
                {
                    _isFiring = false;
                    _magicalRay.SetActive(false);
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_isFiring);
            }
            else
            {
                _isFiring = (bool) stream.ReceiveNext();
            }
        }
    }
}
