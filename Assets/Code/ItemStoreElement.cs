using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class ItemStoreElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _itemElementName;
        [SerializeField] private TMP_Text _itemElementAmount;
        [SerializeField] private Button _buyButton;
        
        public Button BuyButton => _buyButton;

        public void ShowItem(string itemName, string cost)
        {
            _itemElementName.text = itemName;
            _itemElementAmount.text = cost;
        }
    }
}
