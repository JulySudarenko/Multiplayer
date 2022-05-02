using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(()=> LoadScene("Launcher"));
        }

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene("Launcher");
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(()=> LoadScene("Launcher"));
        }
    }
}
