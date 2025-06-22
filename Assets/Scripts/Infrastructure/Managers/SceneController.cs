using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ColorClone.Infrastructure.Managers
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }

        [SerializeField] private float sceneTransitionDelay = 0.1f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadSceneByIndex(int buildIndex)
        {
            StartCoroutine(LoadSceneAsync(buildIndex));
        }

        public void LoadSceneByName(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        public void LoadNextScene()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                StartCoroutine(LoadSceneAsync(nextIndex));
            }
            else
            {
                // Si no hay más escenas, reinicia desde la primera
                StartCoroutine(LoadSceneAsync(0));
            }
        }

        public void RestartCurrentScene()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(LoadSceneAsync(currentIndex));
        }

        private IEnumerator LoadSceneAsync(int buildIndex)
        {
            // Pequeña pausa para permitir que Zenject limpie correctamente
            yield return new WaitForSeconds(sceneTransitionDelay);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // Pequeña pausa para permitir que Zenject limpie correctamente
            yield return new WaitForSeconds(sceneTransitionDelay);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}