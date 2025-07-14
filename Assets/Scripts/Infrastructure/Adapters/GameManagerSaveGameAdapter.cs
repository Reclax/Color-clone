using ColorClone.Domain.Interfaces;
using UnityEngine;
using Services;
using Assets.Scripts.Infrastructure.Managers; // <- importa tus servicios

namespace ColorClone.Infrastructure.Adapters
{
    /// <summary>
    /// Adaptador que implementa ISaveGameRepository usando ProgressService y usuario actual
    /// </summary>
    public class GameManagerSaveGameAdapter : ISaveGameRepository
    {
        private ProgressService progressService;

        public GameManagerSaveGameAdapter()
        {
            var userService = new UserDataService();
            progressService = new ProgressService(userService);
        }

        private string CurrentUser => SessionManager.CurrentUser;

        public bool HasSavedProgress(int slot)
        {
            if (string.IsNullOrEmpty(CurrentUser)) return false;
            int progress = progressService.GetProgress(CurrentUser, slot);
            return progress > 0; // o el criterio que uses para "guardado"
        }

        public int GetSavedLevel(int slot)
        {
            if (string.IsNullOrEmpty(CurrentUser)) return 1;
            return progressService.GetProgress(CurrentUser, slot);
        }

        public void SaveProgress(int slot, int level)
        {
            if (string.IsNullOrEmpty(CurrentUser)) return;
            progressService.SetProgress(CurrentUser, slot, level);
        }

        public void SetCurrentSlot(int slot)
        {
            PlayerPrefs.SetInt("CurrentSlot", slot); // Opcional: puedes guardar el slot actual en otro lado si lo prefieres
        }

        public int GetCurrentSlot()
        {
            return PlayerPrefs.GetInt("CurrentSlot", 0); // O usa tu propio sistema
        }
    }
}