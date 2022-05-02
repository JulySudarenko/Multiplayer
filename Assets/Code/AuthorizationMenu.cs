using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class AuthorizationMenu : MonoBehaviour
    {
        [Header("Registration menu")] 
        [SerializeField] private GameObject _registrationPanel;
        [SerializeField] private InputField _userNameInputField;
        [SerializeField] private InputField _userMailInputField;
        [SerializeField] private InputField _userPasswordInputField;
        [SerializeField] private Button _registrationButton;
        [SerializeField] private Button _deleteAccountButton;

        [Header("Authorization menu")] 
        [SerializeField] private GameObject _authorizationPanel;
        [SerializeField] private InputField _userNameAuthorization;
        [SerializeField] private InputField _userPasswordAuthorization;
        [SerializeField] private Button _authorizationButton;
        [SerializeField] private Button _backButton;

        [SerializeField] private Button _customIdButton;

        private string _userName;
        private string _userMail;
        private string _userPassword;

        public string UserName => _userName;
        public string UserMail => _userMail;
        public string UserPassword => _userPassword;

        public Button DeleteAccountButton => _deleteAccountButton;
        public Button AuthorizationButton => _authorizationButton;
        public Button RegistrationButton => _registrationButton;

        public Button CustomIdButton => _customIdButton;

        public void UpdateUserName(string userName) => _userName = userName;
        public void UpdateUserEmail(string userMail) => _userMail = userMail;
        public void UpdateUserPassword(string userPassword) => _userPassword = userPassword;
        
        public void ChosePanel(bool value)
        {
            if (value)
            {
                 _authorizationPanel.SetActive(true);
                _userNameAuthorization.onEndEdit.AddListener(UpdateUserName);
                _userPasswordAuthorization.onEndEdit.AddListener(UpdateUserPassword);
                _backButton.onClick.AddListener(() => ChosePanel(false));
            }
            else
            {
                _registrationPanel.SetActive(true);
                _userNameInputField.onEndEdit.AddListener(UpdateUserName);
                _userMailInputField.onEndEdit.AddListener(UpdateUserEmail);
                _userPasswordInputField.onEndEdit.AddListener(UpdateUserPassword);
            }

            Dispose(value);
        }

        public void Dispose(bool value)
        {
            if(value)
            {
                _registrationPanel.SetActive(false);
                _userNameInputField.onEndEdit.RemoveAllListeners();
                _userMailInputField.onEndEdit.RemoveAllListeners();
                _userPasswordInputField.onEndEdit.RemoveAllListeners(); 
            }
            else
            {
                _authorizationPanel.SetActive(false);
                _userNameAuthorization.onEndEdit.RemoveAllListeners();
                _userPasswordAuthorization.onEndEdit.RemoveAllListeners();
                _backButton.onClick.RemoveAllListeners();
            }
        }
    }
}
