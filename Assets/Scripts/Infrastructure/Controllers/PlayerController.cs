using System.Collections;
using UnityEngine;
using Zenject;
using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;
using Services;
using Assets.Scripts.Infrastructure.Managers; // Para el SessionManager y ProgressService

namespace ColorClone.Infrastructure.Controllers
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float verticalForce = 400f;
        [SerializeField] private float restartDelay = 1f;

        [Header("Effects")]
        [SerializeField] private ParticleSystem playerParticles;

        [Header("Player Colors")]
        [SerializeField] private Color orangeColor = new Color(1f, 0.5f, 0f, 1f);
        [SerializeField] private Color violetColor = new Color(0.5f, 0f, 1f, 1f);
        [SerializeField] private Color cyanColor = new Color(0f, 1f, 1f, 1f);
        [SerializeField] private Color pinkColor = new Color(1f, 0f, 1f, 1f);

        // Servicios y referencias
        private IInputService _input;
        private IPlayerInteractor _interactor;
        private IPlayerFactory _playerFactory;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private bool _isPlayerActive = true;

        // Servicio para guardar/cargar progreso por usuario
        private ProgressService _progressService;

        // Inyección de dependencias para input y factory
        [Inject]
        public void Construct(IInputService inputService, IPlayerFactory playerFactory)
        {
            _input = inputService;
            _playerFactory = playerFactory;
        }

        /// <summary>
        /// Inicialización de componentes y lógica del jugador al iniciar la escena.
        /// </summary>
        private void Start()
        {
            InitializeComponents();
            InitializeInteractor();
            SubscribeToEvents();
            InitializePlayerColor();

            // Inicializa el ProgressService (usa el sistema de usuarios en JSON)
            _progressService = new ProgressService(new UserDataService());
        }

        /// <summary>
        /// Busca y asigna los componentes requeridos (Rigidbody2D, SpriteRenderer).
        /// </summary>
        private void InitializeComponents()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();

            if (_sr == null)
                Debug.LogError("SpriteRenderer component not found!");
            if (_rb == null)
                Debug.LogError("Rigidbody2D component not found!");
        }

        /// <summary>
        /// Asigna una acción a un botón UI, eliminando listeners previos.
        /// </summary>
        private void SetupButton(UnityEngine.UI.Button button, UnityEngine.Events.UnityAction action)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        /// <summary>
        /// Crea el interactor del jugador usando la fábrica y define los colores y tags.
        /// </summary>
        private void InitializeInteractor()
        {
            var tags = new[] { "Orange", "Violet", "Cyan", "Pink" };
            var colors = new[] {
                new Color(orangeColor.r, orangeColor.g, orangeColor.b, 1f),
                new Color(violetColor.r, violetColor.g, violetColor.b, 1f),
                new Color(cyanColor.r, cyanColor.g, cyanColor.b, 1f),
                new Color(pinkColor.r, pinkColor.g, pinkColor.b, 1f)
            };
            _interactor = _playerFactory.CreatePlayer(_rb, verticalForce, _sr, tags, colors);
        }

        /// <summary>
        /// Suscribe a los eventos del interactor para manejar finalización y muerte del jugador.
        /// </summary>
        private void SubscribeToEvents()
        {
            _interactor.OnFinish += HandleFinishDirect;
            _interactor.OnDie += HandleDieDirect;
        }

        /// <summary>
        /// Selecciona y aplica un color aleatorio al jugador al iniciar el nivel.
        /// Sincroniza el color visual y lógico.
        /// </summary>
        private void InitializePlayerColor()
        {
            Color[] colors = { orangeColor, violetColor, cyanColor, pinkColor };
            int randomIndex = Random.Range(0, colors.Length);
            _sr.color = colors[randomIndex];
            if (_interactor != null)
            {
                var setColorIndexMethod = _interactor.GetType().GetMethod("SetColorIndex");
                if (setColorIndexMethod != null)
                {
                    setColorIndexMethod.Invoke(_interactor, new object[] { randomIndex });
                }
            }
        }

        /// <summary>
        /// Actualización cada frame: detecta salto si el jugador está activo.
        /// </summary>
        private void Update()
        {
            if (_isPlayerActive && _input.GetJumpDown())
                _interactor.Jump();
        }

        /// <summary>
        /// Detecta colisiones (triggers) y delega el manejo al interactor si el jugador está activo.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isPlayerActive)
                _interactor.HandleTrigger(other);
        }

        /// <summary>
        /// Lógica al finalizar el nivel: desactiva el jugador, muestra partículas y guarda el progreso usando JSON.
        /// </summary>
        private void HandleFinishDirect()
        {
            Debug.Log("Player finished level!");

            _isPlayerActive = false;

            if (playerParticles != null)
                Instantiate(playerParticles, transform.position, Quaternion.identity);

            _sr.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.gravityScale = 0f;

            // Guarda el progreso por usuario y slot
            SaveProgressForUserAndSlot();

            // Cambia de escena tras el delay
            Invoke(nameof(LoadNextScene), restartDelay);
        }

        /// <summary>
        /// Guarda el progreso actual del usuario logueado y del slot seleccionado en el sistema JSON.
        /// </summary>
        private void SaveProgressForUserAndSlot()
        {
            // Obtiene el usuario logueado
            string username = SessionManager.CurrentUser;
            if (string.IsNullOrEmpty(username))
            {
                Debug.LogError("No hay usuario logueado en SessionManager.CurrentUser.");
                return;
            }

            // Obtiene el slot actual (puedes adaptarlo si lo guardas en otro lado)
            int slot = GameManager.Instance.GetCurrentSlot();

            // Determina el índice del siguiente nivel
            int nextLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;

            // Guarda el progreso en el JSON
            bool ok = _progressService.SetProgress(username, slot, nextLevel);

            if (ok)
                Debug.Log($"Progreso guardado: Usuario '{username}', Slot {slot}, Nivel {nextLevel}");
            else
                Debug.LogError($"No se pudo guardar progreso para usuario '{username}' en slot {slot}");
        }

        /// <summary>
        /// Lógica al morir el jugador: desactiva, muestra partículas y reinicia el nivel.
        /// </summary>
        private void HandleDieDirect()
        {
            Debug.Log("Player died! Restarting level...");

            _isPlayerActive = false;

            if (playerParticles != null)
                Instantiate(playerParticles, transform.position, Quaternion.identity);

            _sr.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.gravityScale = 0f;

            Invoke(nameof(RestartCurrentScene), restartDelay);
        }

        /// <summary>
        /// Reinicia la escena actual usando el SceneController.
        /// </summary>
        private void RestartCurrentScene()
        {
            ColorClone.Infrastructure.Managers.SceneController.Instance.RestartCurrentScene();
        }

        /// <summary>
        /// Carga la siguiente escena usando el SceneController.
        /// </summary>
        private void LoadNextScene()
        {
            ColorClone.Infrastructure.Managers.SceneController.Instance.LoadNextScene();
        }
    }
}