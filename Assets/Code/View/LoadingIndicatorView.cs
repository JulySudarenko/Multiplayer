using System;
using System.Collections;
using Code.Login;
using TMPro;
using UnityEngine;

namespace Code.View
{
    public class LoadingIndicatorView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _loadingMessage;
        [SerializeField] private GameObject _loadingImage;

        private Coroutine _isWaiting;

        public void ShowLoadingStatusInformation(ConnectionState state, string message)
        {
            switch (state)
            {
                case ConnectionState.Default:
                    _loadingMessage.text = message;
                    _loadingMessage.color = Color.white;
                    break;
                case ConnectionState.Success:
                    _loadingMessage.text = message;
                    _loadingMessage.color = Color.green;
                    StopIndicator();
                    break;
                case ConnectionState.Fail:
                    _loadingMessage.text = message;
                    _loadingMessage.color = Color.red;
                    StopIndicator();
                    break;
                case ConnectionState.Waiting:
                    _loadingMessage.text = message;
                    _loadingMessage.color = Color.yellow;
                    _loadingImage.SetActive(true);
                    _isWaiting = StartCoroutine(ShowWaitingIndicator());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void StopIndicator()
        {
            StopCoroutine(_isWaiting);
            _loadingImage.SetActive(false);
        }

        private IEnumerator ShowWaitingIndicator()
        {
            while (true)
            {
                _loadingImage.transform.Rotate(Vector3.forward, 3.0f);
                yield return new WaitForSeconds(0.01f);
            }
        }

        private void OnDestroy()
        {
            StopIndicator();
        }
    }
}
