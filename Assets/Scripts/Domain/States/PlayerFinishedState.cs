using UnityEngine;
using ColorClone.Application.UseCases;
using ColorClone.Domain.Interfaces;

namespace ColorClone.Domain.States
{
    public class PlayerFinishedState : IPlayerState
    {
        public void Enter(IPlayerContext player)
        {
            player.Finish();
            // Guardar progreso del nivel actual
            int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveLevelProgress(currentScene + 1); // Guarda el siguiente nivel
            }
        }

        public void Exit(IPlayerContext player)
        {
            // Lógica al salir del estado finalizado
        }

        public void Jump(IPlayerContext player)
        {
            // No puede saltar estando finalizado
        }

        public void HandleTrigger(IPlayerContext player, Collider2D other)
        {
            // No procesa triggers estando finalizado
        }
    }
}
