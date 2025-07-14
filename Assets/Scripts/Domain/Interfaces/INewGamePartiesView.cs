using System;

namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Métodos adicionales para selección de partida en modo Nuevo Juego
    /// </summary>
    public interface INewGamePartiesView : IPartiesView
    {
        

        void ShowOverwriteDialog(string message);
        void HideOverwriteDialog();
    }
}