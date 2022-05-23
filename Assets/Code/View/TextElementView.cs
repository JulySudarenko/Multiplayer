using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Code.View
{
    public class TextElementView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void ShowRoom(Player room)
        {
            _text.text = room.UserId;
        }

        public void ShowName(string name)
        {
            _text.text = name;
        }

        public void ShowCurrency(string currency, string amount)
        {
            _text.text = $"{currency} : {amount}";
        }
    }
}
