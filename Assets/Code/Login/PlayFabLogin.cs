using System;
using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Login
{
    public class PlayFabLogin : IDisposable
    {
        private readonly AuthorizationMenu _authorizationMenu;
        private readonly LoadingIndicatorView _loadingIndicatorView;

        private const string AuthKey = "IdKey";
        private string _guid;
        private bool _isIdExist;

        public PlayFabLogin(AuthorizationMenu authorizationMenu, LoadingIndicatorView loadingIndicatorView)
        {
            _authorizationMenu = authorizationMenu;
            _loadingIndicatorView = loadingIndicatorView;
            ActivateMenu();
            _authorizationMenu.DeleteAccountButton.onClick.AddListener(DeleteAccount);
            _authorizationMenu.AuthorizationButton.onClick.AddListener(SignIn);
            _authorizationMenu.RegistrationButton.onClick.AddListener(CreateAccount);
            _authorizationMenu.CustomIdButton.onClick.AddListener(Login);
        }

        private void ActivateMenu()
        {
            _isIdExist = PlayerPrefs.HasKey(AuthKey);
            if (_isIdExist)
            {
                _authorizationMenu.ChosePanel(true);
                _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Default, "Please, sing up");
            }
            else
            {
                _authorizationMenu.ChosePanel(false);
                _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Default, "Please, registry");
            }
        }

        private void CreateAccount()
        {
            _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Waiting, "Waiting for connection...");
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
                {
                    Username = _authorizationMenu.UserName,
                    Email = _authorizationMenu.UserMail,
                    Password = _authorizationMenu.UserPassword,
                    RequireBothUsernameAndEmail = true
                }, result =>
                {
                    _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Success,
                        $"Success: {_authorizationMenu.UserName}");
                    _isIdExist = PlayerPrefs.HasKey(AuthKey);
                    _guid = PlayerPrefs.GetString(AuthKey, Guid.NewGuid().ToString());
                    SceneManager.LoadScene("Profile");
                },
                error =>
                {
                    _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Fail,
                        $"Fail: {error.ErrorMessage}");
                });
        }

        private void SignIn()
        {
            _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Waiting, "Waiting for connection...");
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
                {
                    Username = _authorizationMenu.UserName,
                    Password = _authorizationMenu.UserPassword
                }, result =>
                {
                    _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Success,
                        $"Success: {_authorizationMenu.UserName}");
                    SceneManager.LoadScene("Profile");
                },
                error =>
                {
                    _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Fail,
                        $"Fail: {error.ErrorMessage}");
                });
        }

        private void Login()
        {
            _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Waiting, "Waiting for connection... ");

            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "2402C";
            }

            var request = new LoginWithCustomIDRequest {CustomId = "GeekBrainsLesson3", CreateAccount = _isIdExist};
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult loginResult)
        {
            _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Success,
                $"Connection successful. ID = {_guid}");
            PlayerPrefs.SetString(AuthKey, _guid);
            _isIdExist = PlayerPrefs.HasKey(AuthKey);
            SetUserData();
            //GetUserData(loginResult.PlayFabId);
            SceneManager.LoadScene("Profile");
        }
        
        private void OnLoginFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Fail,
                "Something went wrong: {errorMessage}");
        }

        private void DeleteAccount()
        {
            PlayerPrefs.DeleteKey(AuthKey);
            _loadingIndicatorView.ShowLoadingStatusInformation(ConnectionState.Success,
                "Account was deleted. Please, registry.");
        }

        private void SetUserData() {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
                    Data = new Dictionary<string, string>() 
                    {
                        {"Health", 3.ToString()}
                    }
                },
                result => Debug.Log("Successfully updated user data"),
                error => {
                    Debug.Log("Got error setting user data Ancestor to Arthur");
                    Debug.Log(error.GenerateErrorReport());
                });
        }
        

        public void Dispose()
        {
            _authorizationMenu.Dispose(_isIdExist);
            _authorizationMenu.DeleteAccountButton.onClick.RemoveListener(DeleteAccount);
            _authorizationMenu.AuthorizationButton.onClick.RemoveListener(SignIn);
            _authorizationMenu.RegistrationButton.onClick.RemoveListener(CreateAccount);
            _authorizationMenu.CustomIdButton.onClick.RemoveListener(Login);
        }
    }
}
