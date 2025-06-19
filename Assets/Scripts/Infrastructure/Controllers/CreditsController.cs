using UnityEngine;
using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;

namespace ColorClone.Infrastructure.Controllers
{
    /// <summary>
    /// Controller que conecta la UI de Créditos con la lógica de cierre de juego.
    /// </summary>
    public class CreditsController : MonoBehaviour
    {
        private IGameFlowService _gameFlowService;

        void Awake()
        {
            // Instanciamos nuestro caso de uso de flujo de juego
            _gameFlowService = new GameFlowUseCase();
        }

        /// <summary>
        /// Método público para asignar al botón OnClick de la pantalla de créditos.
        /// </summary>
        public void CloseGame()
        {
            _gameFlowService.QuitApplication();
            Debug.Log("Juego Cerrado");
        }
    }
}
