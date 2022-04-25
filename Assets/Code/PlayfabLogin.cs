using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code
{
    public class PlayfabLogin : MonoBehaviour
    {
        public void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "2402C";
            }

            var request = new LoginWithCustomIDRequest {CustomId = "GeekBrainsLesson3", CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("Congratulations, you made successful API call!");
        }

        private void OnLoginFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }
    }
}
