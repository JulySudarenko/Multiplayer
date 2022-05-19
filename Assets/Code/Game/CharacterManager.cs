using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;


namespace Code.Game
{
    internal class CharacterManager : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private float _health = 1.0f;
        [SerializeField] private GameObject _healthUI;
        [SerializeField] private GameObject _magicalRay;

        private CharacterMovement _movement;
        private CharacterAttack _attack;
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
            
            // var source = GetComponent<AudioSource>();
            // var animator = GetComponent<Animator>();
            // var rigidbody = GetComponent<Rigidbody>();
            //
            // _movement = new CharacterMovement(rigidbody, this.transform, animator, source);
            // _attack = new CharacterAttack();
        }

        private void Start()
        {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
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
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
                //_movement.Update();
            }
            
        }

        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Attack"))
            {
                if (!this._isFiring)
                {
                    this._isFiring = true;
                    _magicalRay.SetActive(true);
                }
            }

            if (Input.GetButtonUp("Attack"))
            {
                if (this._isFiring)
                {
                    this._isFiring = false;
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
                this._isFiring = (bool) stream.ReceiveNext();
            }
        }
    }

    internal class CharacterAttack
    {
        
    }
}
