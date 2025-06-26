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
        
        [Header("Scene Configuration")]
        [SerializeField] private string partiesSceneName = "Parties";

        private const string LastLevelKey = "LastLevelIndex";

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
            // Configurar modo nueva partida y cargar escena de partidas
            ColorClone.Presentation.Unity.PartiesPresenter.SetGlobalNewGameMode(true);
            SceneManager.LoadScene(partiesSceneName);
        }

        public void OnContinueButton()
        {
            // Configurar modo continuar y cargar escena de partidas
            ColorClone.Presentation.Unity.PartiesPresenter.SetGlobalNewGameMode(false);
            SceneManager.LoadScene(partiesSceneName);
        }

        public void OnExitButton()
        {
            UnityEngine.Application.Quit();
        }
    }
}
