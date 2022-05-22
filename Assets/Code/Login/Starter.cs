using Code.View;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Login
{
    public class Starter : MonoBehaviour
    {
        [SerializeField] private AuthorizationMenu _authorizationMenu;
        [FormerlySerializedAs("_loadingIndicatorWidget")] [SerializeField] private LoadingIndicatorView _loadingIndicatorView;

        private PlayFabLogin _playFabLogin;

        private void Awake()
        {
            _playFabLogin = new PlayFabLogin(_authorizationMenu, _loadingIndicatorView);
        }
    }
}
