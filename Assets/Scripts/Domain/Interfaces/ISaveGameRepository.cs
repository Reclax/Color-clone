using System;

namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Interface para el repositorio de partidas guardadas
    /// Abstrae el acceso a datos siguiendo el patrón Repository
    /// </summary>
    public interface ISaveGameRepository
    {
        bool HasSavedProgress(int slot);
        int GetSavedLevel(int slot);
        void SaveProgress(int slot, int level);
        void SetCurrentSlot(int slot);
        int GetCurrentSlot();
    }
}
