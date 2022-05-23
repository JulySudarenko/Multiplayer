using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Catalog
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _gameRoomsButton;
        [SerializeField] private Button _profileButton;
        [SerializeField] private GameObject _gameRoomsPanel;
        [SerializeField] private GameObject _profilePanel;

        [SerializeField] private CurrencyStoreElement _gold;
        [SerializeField] private CurrencyStoreElement _experience;
        [SerializeField] private Transform _itemListSlider;
        [SerializeField] private ItemStoreElement _item;

        private CatalogManager _catalogManager;

        private void Awake()
        {
            _backButton.onClick.AddListener(() => LoadScene("Launcher"));
            _gameRoomsButton.onClick.AddListener(GoToRoomsList);
            _profileButton.onClick.AddListener(GoToProfile);
            GoToProfile();
            
            _catalogManager = GetComponent<CatalogManager>();
            _catalogManager.CreateCatalog(_gold, _experience, _itemListSlider, _item);
        }

        private void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        private void GoToRoomsList()
        {
            _gameRoomsPanel.SetActive(true);
            _profilePanel.SetActive(false);
        }

        private void GoToProfile()
        {
            _gameRoomsPanel.SetActive(false);
            _profilePanel.SetActive(true);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveAllListeners();
            _gameRoomsButton.onClick.RemoveAllListeners();
            _profileButton.onClick.RemoveAllListeners();

            _catalogManager.OnDestroy();
        }
    }
}
