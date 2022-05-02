using UnityEngine;

namespace Code
{
    public class Starter : MonoBehaviour
    {
        [SerializeField] private AuthorizationMenu _authorizationMenu;
        [SerializeField] private LoadingIndicatorWidget _loadingIndicatorWidget;

        private PlayFabLogin _playFabLogin;

        private void Awake()
        {
            _playFabLogin = new PlayFabLogin(_authorizationMenu, _loadingIndicatorWidget);
        }
    }
}
