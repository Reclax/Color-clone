using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColorClone.Infrastructure.Managers
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }

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
            SceneManager.LoadScene(buildIndex);
        }

        public void LoadSceneByName(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadNextScene()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex);
            }
            else
            {
                // Si no hay más escenas, reinicia desde la primera
                SceneManager.LoadScene(0);
            }
        }

        public void RestartCurrentScene()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex);
        }
    }
}

