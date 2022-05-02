using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class ButtonColorWidget : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private Image _buttonImage;
        
        public Button ColoringButton => _button;

        public void ChangeColor(ConnectionState state, string connectionMessage)
        {
            switch (state)
            {
                case ConnectionState.Default:
                    _buttonImage.color = Color.blue;
                    break;
                case ConnectionState.Success:
                    _buttonImage.color = Color.cyan;
                    break;
                case ConnectionState.Fail:
                    _buttonImage.color = Color.red;
                    break;
                case ConnectionState.Waiting:
                    _buttonImage.color = Color.green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            _message.text = connectionMessage;
        }
    }
}
