using Photon.Pun;
using UnityEngine;


namespace Code.Game
{
    internal class CharacterAnimatorController : MonoBehaviourPun 
    {
        [SerializeField] private float _directionDampTime = 0.25f;
        private Animator _animator;
        private Camera _camera;
        private float _horizontal = 0.0f;
        private float _vertical = 0.0f;

        #region MonoBehaviour CallBacks

        void Start () 
        {
            _animator = GetComponent<Animator>();
            _camera = Camera.main;
        }

        void Update () 
        {

            if( photonView.IsMine == false && PhotonNetwork.IsConnected == true )
            {
                return;
            }

            if (!_animator)
            {
                return;
            }

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);			

            if (stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetButtonDown("Jump")) _animator.SetTrigger("Jump"); 
            }
           
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    var direction = hit.point - transform.position;
                    _horizontal = direction.x;
                    _vertical = direction.z;
                }
            }
            else
            {
                _horizontal = Input.GetAxis("Horizontal");
                _vertical = Input.GetAxis("Vertical");
            }

            if( _vertical < 0 )
            {
                _vertical = 0;
            }

            _animator.SetFloat( "Speed", _horizontal*_horizontal+_vertical*_vertical );
            _animator.SetFloat( "Direction", _horizontal, _directionDampTime, Time.deltaTime );
        }

        #endregion

    }
}
