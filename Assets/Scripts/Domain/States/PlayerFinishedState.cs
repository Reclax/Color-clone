using Assets.Scripts.Infrastructure.Managers;
using ColorClone.Domain.Interfaces;
using Services;
using UnityEngine;

namespace ColorClone.Domain.States
{
    public class PlayerFinishedState : IPlayerState
    {
        public void Enter(IPlayerContext player)
        {
            player.Finish();

            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            int currentSceneIndex = currentScene.buildIndex;

            // Guarda el progreso del slot actual en el usuario actual
            if (SessionManager.currentUser != null)
            {
                int slot = SessionManager.CurrentSlot;

                // Asegúrate que la lista de progreso tenga suficientes slots
                if (slot >= 0 && slot < SessionManager.currentUser.progress.Count)
                {
                    SessionManager.currentUser.progress[slot] = currentSceneIndex;
                    new UserDataService().UpdateUser(SessionManager.currentUser);

                    Debug.Log($"Progreso guardado correctamente: Usuario '{SessionManager.CurrentUser}', Slot {slot}, Nivel {currentSceneIndex}");
                }
                else
                {
                    Debug.LogWarning($"Slot inválido para guardar progreso: {slot}");
                }
            }
            else
            {
                Debug.LogWarning("No hay usuario logueado para guardar progreso.");
            }
        }

        public void Exit(IPlayerContext player) { }
        public void Jump(IPlayerContext player) { }
        public void HandleTrigger(IPlayerContext player, Collider2D other) { }
    }
}