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
        [SerializeField] private UnityEngine.UI.Button btnNewGame;
        [SerializeField] private UnityEngine.UI.Button btnContinue;
        [SerializeField] private UnityEngine.UI.Button btnExit;

        private const string LastLevelKey = "LastLevelIndex";
        private int firstLevelBuildIndex = 1; // Ajusta si tu primer nivel tiene otro índice

        [Inject]
        public void Construct()
        {
            // Inject additional services/use-cases here if needed
        }

        private void Start()
        {
            btnNewGame.onClick.AddListener(OnNewGameButton);
            btnContinue.onClick.AddListener(OnContinueButton);
            btnExit.onClick.AddListener(OnExitButton);

            // Desactivar continuar si no hay progreso guardado
            btnContinue.interactable = GameManager.Instance != null && GameManager.Instance.HasSavedProgress();
        }

        /// <summary>
        /// Hook this to your UI Button's OnClick.
        /// </summary>
        public void OnNewGameButton()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveLevelProgress(firstLevelBuildIndex);
            }
            _startPanel.SetActive(false);
            SceneManager.LoadScene(firstLevelBuildIndex);
        }

        public void OnContinueButton()
        {
            if (GameManager.Instance != null && GameManager.Instance.HasSavedProgress())
            {
                int lastLevel = GameManager.Instance.GetSavedLevel();
                _startPanel.SetActive(false);
                SceneManager.LoadScene(lastLevel);
            }
        }

        public void OnExitButton()
        {
            UnityEngine.Application.Quit();
        }
    }
}
