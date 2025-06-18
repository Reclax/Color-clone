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

            // Verificar que el SpriteRenderer esté habilitado
            if (_sr != null)
            {
                _sr.enabled = true;
                Debug.Log($"SpriteRenderer enabled: {_sr.enabled}");
                Debug.Log($"SpriteRenderer sprite: {(_sr.sprite != null ? _sr.sprite.name : "NULL")}");
            }
            else
            {
                Debug.LogError("SpriteRenderer component not found!");
            }
        }

        private void InitializeInteractor()
        {
            // Parallel arrays of tags and colors
            var tags = new[] { "Orange", "Violet", "Cyan", "Pink" };

            // Forzar alpha a 1.0 para todos los colores (corrección temporal)
            var colors = new[] {
                new Color(orangeColor.r, orangeColor.g, orangeColor.b, 1f),
                new Color(violetColor.r, violetColor.g, violetColor.b, 1f),
                new Color(cyanColor.r, cyanColor.g, cyanColor.b, 1f),
                new Color(pinkColor.r, pinkColor.g, pinkColor.b, 1f)
            };

            // Debug de los colores después de la corrección
            Debug.Log("Colors after alpha correction:");
            for (int i = 0; i < colors.Length; i++)
            {
                Debug.Log($"Color {i} ({tags[i]}): {colors[i]}");
            }

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
            _interactor.OnFinish += () => StartCoroutine(HandleFinish());
            _interactor.OnDie += () => StartCoroutine(HandleDie());
        }

        private void InitializePlayerColor()
        {
            // Initialize player color (esto faltaba en la versión original)
            _interactor.ChangeColor();

            // Debug para verificar el color aplicado
            Debug.Log($"Player color initialized: {_sr.color}");
            Debug.Log($"Player sprite: {(_sr.sprite != null ? _sr.sprite.name : "NULL")}");

            // Fallback: forzar un color visible si algo está mal
            if (_sr.color.a < 0.1f) // Si es muy transparente
            {
                _sr.color = Color.white; // Forzar color blanco visible
                Debug.LogWarning("Forced white color due to transparency issues");
            }
        }

        private void Update()
        {
            if (_input.GetJumpDown())
                _interactor.Jump();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _interactor.HandleTrigger(other);
        }

        private IEnumerator HandleFinish()
        {
            // Desactivar el jugador primero
            gameObject.SetActive(false);

            // Instanciar las partículas en la posición del jugador (como en el código original)
            if (playerParticles != null)
                Instantiate(playerParticles, transform.position, Quaternion.identity);

            // Esperar antes de cargar la siguiente escena
            yield return new WaitForSeconds(restartDelay);

            // Cargar la siguiente escena
            int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(activeSceneIndex + 1);
        }

        private IEnumerator HandleDie()
        {
            // Desactivar el jugador primero
            gameObject.SetActive(false);

            // Instanciar las partículas en la posición del jugador (como en el código original)
            if (playerParticles != null)
                Instantiate(playerParticles, transform.position, Quaternion.identity);

            // Esperar antes de reiniciar la escena
            yield return new WaitForSeconds(restartDelay);

            // Reiniciar la escena actual
            int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(activeSceneIndex);
        }
    }
}