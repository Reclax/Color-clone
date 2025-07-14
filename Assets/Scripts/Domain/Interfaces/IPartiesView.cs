using System;

namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Métodos y eventos comunes para cualquier selección de partida (nuevo o continuar)
    /// </summary>
    public interface IPartiesView
    {
        

        void DisplayTitle(string title);
        void DisplaySlotInfo(int slotIndex, string text, bool hasProgress);
    }
}