using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using ColorClone.Domain.Interfaces;

namespace ColorClone.Application.UseCases
{
    /// <summary>
    /// Caso de uso que implementa el cierre de la aplicación.
    /// </summary>
    public class GameFlowUseCase : IGameFlowService
    {
        public void QuitApplication()
        {
#if UNITY_EDITOR
            // Detiene el modo Play en el Editor para simular el cierre
            EditorApplication.isPlaying = false;
#else
            // Cierra la build final del juego
            Application.Quit();
#endif
        }
    }
}
