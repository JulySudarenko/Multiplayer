using UnityEngine;

namespace Code.Game
{
    public class CharacterMovement
    {
        private Animator m_Animator;
        private Transform m_Transform;
        private Rigidbody m_Rigidbody;
        private Quaternion m_Rotation = Quaternion.identity;
        private AudioSource _source;
        private Vector3 m_Movement;
        private readonly float _turnSpeed = 20f;
        private readonly float _speed = 1f;

        public CharacterMovement(Rigidbody rigidbody, Transform transform, Animator animator, AudioSource source)
        {
            m_Rigidbody = rigidbody;
            m_Animator = animator;
            m_Transform = transform;
            _source = source;
        }

        public void Update()
        {
            float horizontal = Input.GetAxis(Names.HORIZONTAL);
            float vertical = Input.GetAxis(Names.VERTICAL);

            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize();

            Vector3 desiredForward =
                Vector3.RotateTowards(m_Transform.forward, m_Movement, _turnSpeed * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);

            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * (Time.deltaTime * _speed));
            Debug.Log(m_Rigidbody.position + m_Movement * (Time.deltaTime * _speed));
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * _speed);//m_Animator.deltaPosition.magnitude * _speed);
            m_Rigidbody.MoveRotation(m_Rotation);
        }

        private void OnAnimatorMove()
        {
            // m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * speed);
            // m_Rigidbody.MoveRotation(m_Rotation);
        }

        private void StepVoice()
        {
            //SoundLibrary.PlaySound(source, SoundLibrary.Sound.Step);
        }
    }
}
