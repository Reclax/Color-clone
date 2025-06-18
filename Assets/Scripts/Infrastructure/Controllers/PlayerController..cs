using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;

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
        [SerializeField] private Color orangeColor = new Color(1f, 0.5f, 0f, 1f); // Orange con alpha completo
        [SerializeField] private Color violetColor = new Color(0.5f, 0f, 1f, 1f); // Violet con alpha completo
        [SerializeField] private Color cyanColor = new Color(0f, 1f, 1f, 1f);     // Cyan con alpha completo
        [SerializeField] private Color pinkColor = new Color(1f, 0f, 1f, 1f);     // Pink con alpha completo

        private IInputService _input;
        private IPlayerInteractor _interactor;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private bool _isPlayerActive = true;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _input = inputService;
        }

        private void Start()
        {
            InitializeComponents();
            InitializeInteractor();
            SubscribeToEvents();
            InitializePlayerColor();
        }

        private void InitializeComponents()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();

            // Verificar componentes críticos
            if (_sr == null)
                Debug.LogError("SpriteRenderer component not found!");
            if (_rb == null)
                Debug.LogError("Rigidbody2D component not found!");
        }

        private void InitializeInteractor()
        {
            // Parallel arrays of tags and colors
            var tags = new[] { "Orange", "Violet", "Cyan", "Pink" };

            // Forzar alpha a 1.0 para todos los colores
            var colors = new[] {
                new Color(orangeColor.r, orangeColor.g, orangeColor.b, 1f),
                new Color(violetColor.r, violetColor.g, violetColor.b, 1f),
                new Color(cyanColor.r, cyanColor.g, cyanColor.b, 1f),
                new Color(pinkColor.r, pinkColor.g, pinkColor.b, 1f)
            };

            // Instantiate the use-case
            _interactor = new PlayerUseCase(
                _rb,
                verticalForce,
                _sr,
                tags,
                colors
            );
        }

        private void SubscribeToEvents()
        {
            // Subscribe to domain events to handle finish & death
            _interactor.OnFinish += HandleFinishDirect;
            _interactor.OnDie += HandleDieDirect;
        }

        private void InitializePlayerColor()
        {
            // Initialize player color
            _interactor.ChangeColor();
        }

        private void Update()
        {
            // Solo procesar input si el jugador está activo
            if (_isPlayerActive && _input.GetJumpDown())
                _interactor.Jump();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Solo procesar triggers si el jugador está activo
            if (_isPlayerActive)
                _interactor.HandleTrigger(other);
        }

        private void HandleFinishDirect()
        {
            Debug.Log("Player finished level!");

            // Desactivar el jugador inmediatamente para evitar más inputs
            _isPlayerActive = false;

            // Instanciar las partículas inmediatamente
            if (playerParticles != null)
                Instantiate(playerParticles, transform.position, Quaternion.identity);

            // Desactivar visualmente el jugador
            _sr.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.gravityScale = 0f;

            // Cargar siguiente escena después del delay usando Invoke
            Invoke(nameof(LoadNextScene), restartDelay);
        }

        private void HandleDieDirect()
        {
            Debug.Log("Player died! Restarting level...");

            // Desactivar el jugador inmediatamente para evitar más inputs
            _isPlayerActive = false;

            // Instanciar las partículas inmediatamente
            if (playerParticles != null)
                Instantiate(playerParticles, transform.position, Quaternion.identity);

            // Desactivar visualmente el jugador
            _sr.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.gravityScale = 0f;

            // Reiniciar la escena después del delay usando Invoke
            Invoke(nameof(RestartCurrentScene), restartDelay);
        }

        private void RestartCurrentScene()
        {
            int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            Debug.Log($"Restarting scene: {activeSceneIndex}");
            SceneManager.LoadScene(activeSceneIndex);
        }

        private void LoadNextScene()
        {
            int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = activeSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log($"Loading next scene: {nextSceneIndex}");
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No more levels! Restarting from first level");
                SceneManager.LoadScene(0);
            }
        }
    }
}