using Assets.Scripts.Infrastructure.Managers;
using ColorClone.Domain.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColorClone.Presentation.Unity
{ 
    public class MenuScreenPresenter :MonoBehaviour
    {
        private IGameFlowService _gameFlowService;
       
        public void OnClickNewGame()
        {
            SceneManager.LoadScene("SelectNewGameSlots");
        }
        public void OnClickContinue()
        {
            SceneManager.LoadScene("SelectContinueSlots");
        }
        public void OnClickChangePassword()
        {
            SceneManager.LoadScene("ChangePasswordScene");
        }
        public void OnClickCloseSesion()
        {
            // Cierra la sesión del usuario actual
            SessionManager.Logout();
            // Regresa a la pantalla de login
            SceneManager.LoadScene("LoginScene");
        }
        /// <summary>
        /// Acción para salir del juego.
        /// </summary>
        public void OnExitButton()
        {
            if (_gameFlowService != null)
            {
                _gameFlowService.QuitApplication();
            }
            else
            {
                OnClickCloseSesion();
                UnityEngine.Application.Quit();
            }
        }

    }
}
