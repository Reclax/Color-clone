using UnityEngine;
using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;

namespace ColorClone.Infrastructure.Controllers
{
    /// <summary>
    /// Controller que conecta la UI de Cr�ditos con la l�gica de cierre de juego.
    /// </summary>
    public class CreditsController : MonoBehaviour
    {
        private IGameFlowService _gameFlowService;

        void Awake()
        {
            // Instanciamos nuestro caso de uso de flujo de juego
            _gameFlowService = new GameFlowUseCase();
        }

        private void SetupButton(UnityEngine.UI.Button button, UnityEngine.Events.UnityAction action)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        /// <summary>
        /// M�todo p�blico para asignar al bot�n OnClick de la pantalla de cr�ditos.
        /// </summary>
        public void CloseGame()
        {
            _gameFlowService.QuitApplication();
            Debug.Log("Juego Cerrado");
        }
    }
}
