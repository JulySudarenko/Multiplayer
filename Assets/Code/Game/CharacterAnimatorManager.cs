using Photon.Pun;
using UnityEngine;

namespace Code.Game
{
    internal class CharacterAnimatorManager : MonoBehaviourPun
    {
        private Animator _animator;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (!_animator)
            {
                return;
            }

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (Input.GetButtonDown(Names.JUMP)) _animator.SetTrigger(Names.JUMP);
            if (Input.GetButtonDown(Names.ATTACK)) _animator.SetTrigger(Names.ATTACK);

            float horizontal = Input.GetAxis(Names.HORIZONTAL);
            float vertical = Input.GetAxis(Names.VERTICAL);

            if (vertical < 0)
            {
                vertical = 0;
            }

            _animator.SetFloat(Names.SPEED, horizontal * horizontal + vertical * vertical);
        }
    }

    public static class Names
    {
        public const string SPEED = "Speed";
        public const string JUMP = "Jump";
        public const string ATTACK = "Attack";
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";
    }
}
