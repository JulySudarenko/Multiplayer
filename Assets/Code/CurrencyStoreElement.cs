using TMPro;
using UnityEngine;

namespace Code
{
    public class CurrencyStoreElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currencyElement;

        public void ShowCurrency(string currency, string amount)
        {
            _currencyElement.text = $"{currency} : {amount}";
        }
    }
}
