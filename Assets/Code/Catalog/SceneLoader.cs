using Code.View;
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

        [SerializeField] private PlayerNamePanelView _enterNamePanel;
        [SerializeField] private Transform _shopPanel;
        [SerializeField] private Transform _inventoryPanel;
        [SerializeField] private Transform _charactersPanel;
        [SerializeField] private LineElementView _lineElement;
        [SerializeField] private TextElementView _gold;
        [SerializeField] private TextElementView _experience;
        [SerializeField] private TextElementView _inventoryItem;


        private void Awake()
        {
            _backButton.onClick.AddListener(() => LoadScene("Launcher"));
            _gameRoomsButton.onClick.AddListener(GoToRoomsList);
            _profileButton.onClick.AddListener(GoToProfile);
            GoToProfile();

            var catalogManager =
                new CatalogManager(_shopPanel, _inventoryPanel, _charactersPanel, _enterNamePanel, 
                    _gold, _experience, _lineElement);
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
        }
    }
}
