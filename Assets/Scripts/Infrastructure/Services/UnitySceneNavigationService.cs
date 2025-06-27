using ColorClone.Domain.Interfaces;
using UnityEngine.SceneManagement;

namespace ColorClone.Infrastructure.Services
{
    /// <summary>
    /// Servicio de navegación que usa Unity SceneManager
    /// Implementa el patrón Strategy para navegación
    /// </summary>
    public class UnitySceneNavigationService : ISceneNavigationService
    {
        private readonly string _previousSceneName;

        public UnitySceneNavigationService(string previousSceneName = "StartScreen")
        {
            _previousSceneName = previousSceneName;
        }

        public void NavigateToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void NavigateToScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        public void GoBack()
        {
            SceneManager.LoadScene(_previousSceneName);
        }
    }
}
