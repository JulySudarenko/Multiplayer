﻿using UnityEngine;
using UnityEngine.UI;


namespace Code.Game
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Vector3 _screenOffset = new Vector3(0f, 30f, 0f);
        [SerializeField] private Text _playerNameText;
        [SerializeField] private Slider _playerHealthSlider;
        private CharacterController _target;
        private Renderer _targetRenderer;
        private Transform _targetTransform;
        private CanvasGroup _canvasGroup;
        private Vector3 _targetPosition;
        private float _characterControllerHeight;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        void Update()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }

            if (_playerHealthSlider != null)
            {
                _playerHealthSlider.value = _target.Health;
            }
        }

        void LateUpdate()
        {
            if (_targetRenderer != null)
            {
                _canvasGroup.alpha = _targetRenderer.isVisible ? 1f : 0f;
            }

            if (_targetTransform != null)
            {
                _targetPosition = _targetTransform.position;
                _targetPosition.y += _characterControllerHeight;

                transform.position = Camera.main.WorldToScreenPoint(_targetPosition) + _screenOffset;
            }
        }

        public void SetTarget(CharacterController target)
        {
            if (target == null)
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> PlayMakerManager target for PlayerUI.SetTarget.",
                    this);
                return;
            }

            _target = target;
            _targetTransform = target.GetComponent<Transform>();
            _targetRenderer = target.GetComponentInChildren<Renderer>();

            UnityEngine.CharacterController _characterController = target.GetComponent<UnityEngine.CharacterController>();

            if (_characterController != null)
            {
                _characterControllerHeight = _characterController.height;
            }

            if (_playerNameText != null)
            {
                _playerNameText.text = _target.photonView.Owner.NickName;
                Debug.Log(_target.photonView.Owner.NickName);
            }
        }
    }
}
