using System;

namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Interface para el servicio de navegación entre escenas
    /// Siguiendo el patrón Strategy para diferentes tipos de navegación
    /// </summary>
    public interface ISceneNavigationService
    {
        void NavigateToScene(string sceneName);
        void NavigateToScene(int buildIndex);
        void GoBack();
    }
}
