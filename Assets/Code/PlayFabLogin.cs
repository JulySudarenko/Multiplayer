using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code
{
    public class PlayFabLogin : MonoBehaviour
    {
        [SerializeField] private ButtonColorWidget _button;

        public void Awake()
        {
            _button.ChangeColor(ConnectionState.Default, "Press to connected");
            _button.ColoringButton.onClick.AddListener(Login);
        }

        private void Login()
        {
            _button.ChangeColor(ConnectionState.Waiting, "Waiting for connection...");
            
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "2402C";
                Debug.Log("Set TitleID");
            }

            var request = new LoginWithCustomIDRequest {CustomId = "GeekBrainsLesson3", CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            _button.ChangeColor(ConnectionState.Success, "Connection successful");
            Debug.Log("Congratulations, you made successful API call!");
        }

        private void OnLoginFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            _button.ChangeColor(ConnectionState.Fail, $"Something went wrong: {errorMessage}");
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

        private void OnDestroy()
        {
            _button.ColoringButton.onClick.RemoveAllListeners();
        }
    }
}
