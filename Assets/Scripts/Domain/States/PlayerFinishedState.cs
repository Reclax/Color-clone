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
            // Guardar progreso solo si NO es la EndScreen
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            int nextSceneIndex = currentScene.buildIndex + 1;
            string nextSceneName = (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
                ? System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(nextSceneIndex))
                : string.Empty;
            Debug.Log($"Intentando guardar progreso. Escena actual: {currentScene.name}, siguiente: {nextSceneName}");
            if (GameManager.Instance != null && nextSceneName != "EndScreen")
            {
                GameManager.Instance.SaveLevelProgress(nextSceneIndex);
                Debug.Log($"Progreso guardado para el nivel: {nextSceneIndex}");
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
