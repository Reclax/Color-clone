using System;

namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Interface que define las operaciones que debe implementar la vista de partidas
    /// Siguiendo el patrón MVP (Model-View-Presenter)
    /// </summary>
    public interface IPartiesView
    {
        event Action<int> OnSlotSelected;
        event Action OnBackRequested;
        event Action OnOverwriteConfirmed;
        event Action OnOverwriteCancelled;

        void DisplayTitle(string title);
        void DisplaySlotInfo(int slotIndex, string text, bool hasProgress);
        void ShowOverwriteDialog(string message);
        void HideOverwriteDialog();
    }
}
