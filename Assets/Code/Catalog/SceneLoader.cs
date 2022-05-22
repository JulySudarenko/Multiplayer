﻿using Code.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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

        [SerializeField] private TextElementView _gold;
        [SerializeField] private TextElementView _experience;
        [SerializeField] private TextElementView _inventoryItem;
        [SerializeField] private Transform _shopSlider;
        [SerializeField] private Transform _inventorySlider;
        [SerializeField] private ItemStoreElementView _item;

        private void Awake()
        {
            _backButton.onClick.AddListener(() => LoadScene("Launcher"));
            _gameRoomsButton.onClick.AddListener(GoToRoomsList);
            _profileButton.onClick.AddListener(GoToProfile);
            GoToProfile();

            var catalogManager =
                new CatalogManager(_gold, _experience, _shopSlider, _item, _inventorySlider, _inventoryItem);
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
