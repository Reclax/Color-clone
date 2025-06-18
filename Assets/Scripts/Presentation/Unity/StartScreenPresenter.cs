using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ColorClone.Presentation.Unity
{
    /// <summary>
    /// Handles the start-screen UI and begins the game.
    /// </summary>
    public class StartScreenPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _startPanel;

        [Inject]
        public void Construct()
        {
            // Inject additional services/use-cases here if needed
        }

        /// <summary>
        /// Hook this to your UI Button's OnClick.
        /// </summary>
        public void OnStartButton()
        {
            _startPanel.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            // Optionally broadcast a GameStarted event here.
        }
    }
}
