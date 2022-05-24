using System;
using System.Collections.Generic;
using Code.View;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Game
{
    public class MyGameManager : MonoBehaviour
    {
        private List<LineElementView> _lineElements = new List<LineElementView>();
        [SerializeField] private GameObject _robotPrefab;
        [SerializeField] private GameObject _magPrefab;
        [SerializeField] private Transform _spawnPlaces;
        [SerializeField] private Transform _characterSelectedPanel;
        [SerializeField] private LineElementView _lineElement;

        private GameObject _selectedCharacter;
        private CharacterSpawner _characterSpawner;

        private void Awake()
        {
            ShowAllUserCharacters();
            _characterSpawner = new CharacterSpawner(_spawnPlaces);
        }

        private void ShowAllUserCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    foreach (var character in result.Characters)
                    {
                        var characterLine = Object.Instantiate(_lineElement, _characterSelectedPanel);
                        characterLine.gameObject.SetActive(true);
                        characterLine.TextUp.text = $"{character.CharacterName} {character.CharacterType}";
                        characterLine.Button.onClick.AddListener(() => SelectCharacter(character.CharacterType));
                        UpdateCharacterView(character.CharacterId, characterLine.TextDown);
                        _lineElements.Add(characterLine);
                    }
                }, Debug.LogError);
        }

        private void UpdateCharacterView(string characterId, TMP_Text text)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                {
                    CharacterId = characterId
                },
                result =>
                {
                    text.text = $"<b>Level</b>\n{result.CharacterStatistics["Level"]}" +
                                $"\n<b>Experience</b>\n{result.CharacterStatistics["Exp"]}" +
                                $"\n<b>Health</b>\n{result.CharacterStatistics["Health"]}" +
                                $"\n<b>Damage</b>\n{result.CharacterStatistics["Damage"]}" +
                                $"\n<b>Gold </b>\n{result.CharacterStatistics["Gold"]}";
                },
                Debug.LogError);
        }

        private void SelectCharacter(string type)
        {
            switch (type)
            {
                case "ch01":
                    _selectedCharacter = _robotPrefab;
                    break;
                case "ch02":
                    _selectedCharacter = _magPrefab;
                    break;
                default:
                    Debug.Log($"No character with type {type}");
                    break;
            }

            _characterSpawner.SpawnSelectedCharacter(_selectedCharacter);
            _characterSelectedPanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _lineElements.Count; i++)
            {
                _lineElements[i].Button.onClick.RemoveAllListeners();
            }
        }
    }
}
