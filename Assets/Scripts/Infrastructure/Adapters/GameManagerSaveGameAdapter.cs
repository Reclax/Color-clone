using ColorClone.Domain.Interfaces;
using UnityEngine;

namespace ColorClone.Infrastructure.Adapters
{
    /// <summary>
    /// Adaptador que implementa ISaveGameRepository usando GameManager
    /// Sigue el patrón Adapter para separar la infraestructura del dominio
    /// </summary>
    public class GameManagerSaveGameAdapter : ISaveGameRepository
    {
        public bool HasSavedProgress(int slot)
        {
            Debug.Log($"[Adapter] Consultando si hay progreso en slot {slot}");
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null!");
                return false;
            }
            return GameManager.Instance.HasSavedProgressInSlot(slot);
        }

        public int GetSavedLevel(int slot)
        {
            Debug.Log($"[Adapter] Consultando nivel guardado en slot {slot}");
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null!");
                return 1;
            }
            return GameManager.Instance.GetSavedLevelFromSlot(slot);
        }

        public void SaveProgress(int slot, int level)
        {
            Debug.Log($"[Adapter] Guardando progreso en slot {slot}, nivel {level}");
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null!");
                return;
            }
            GameManager.Instance.SaveLevelProgressToSlot(slot, level);
        }

        public void SetCurrentSlot(int slot)
        {
            Debug.Log($"[Adapter] Seteando slot actual a {slot}");
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null!");
                return;
            }
            GameManager.Instance.SetCurrentSlot(slot);
        }

        public int GetCurrentSlot()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null!");
                return 0;
            }
            return GameManager.Instance.GetCurrentSlot();
        }
    }
}
