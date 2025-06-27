using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;
using Zenject;

namespace ColorClone.Application.Presenters
{
    /// <summary>
    /// Presenter que maneja la lógica de presentación para la pantalla de partidas
    /// Sigue el patrón MVP y los principios de Clean Architecture
    /// </summary>
    public class PartiesScreenPresenter
    {
        private IPartiesView _view;
        private readonly SlotSelectionUseCase _slotSelectionUseCase;
        private readonly ISaveGameRepository _saveGameRepository;
        private readonly ISceneNavigationService _navigationService;

        private int _selectedSlot = -1;
        private bool _isNewGameMode = false;
        private int _firstLevelBuildIndex = 1;

        [Inject]
        public PartiesScreenPresenter(
            SlotSelectionUseCase slotSelectionUseCase,
            ISaveGameRepository saveGameRepository,
            ISceneNavigationService navigationService)
        {
            _slotSelectionUseCase = slotSelectionUseCase;
            _saveGameRepository = saveGameRepository;
            _navigationService = navigationService;
        }

        public void SetView(IPartiesView view)
        {
            _view = view;
            SubscribeToViewEvents();
        }

        public void Initialize(bool isNewGameMode, int firstLevelBuildIndex = 1)
        {
            UnityEngine.Debug.Log($"PartiesScreenPresenter: Initialize called - isNewGameMode: {isNewGameMode}");
            
            _isNewGameMode = isNewGameMode;
            _firstLevelBuildIndex = firstLevelBuildIndex;

            // Si no hay partidas guardadas, forzar modo nueva partida
            if (!_isNewGameMode && !HasAnyProgress())
            {
                UnityEngine.Debug.Log("PartiesScreenPresenter: No hay progreso, forzando modo nueva partida");
                _isNewGameMode = true;
            }

            UnityEngine.Debug.Log($"PartiesScreenPresenter: Final mode - isNewGameMode: {_isNewGameMode}");
            UpdateUI();
        }

        private void SubscribeToViewEvents()
        {
            _view.OnSlotSelected += HandleSlotSelection;
            _view.OnBackRequested += HandleBackRequest;
            _view.OnOverwriteConfirmed += HandleOverwriteConfirmed;
            _view.OnOverwriteCancelled += HandleOverwriteCancelled;
        }

        private void UpdateUI()
        {
            // Actualizar título
            string title = _isNewGameMode ? 
                "Nueva Partida - Seleccionar Slot" : 
                "Continuar Partida - Seleccionar Slot";
            _view.DisplayTitle(title);

            // Actualizar información de slots
            UpdateSlotsDisplay();
        }

        private void UpdateSlotsDisplay()
        {
            for (int i = 0; i < 3; i++) // Asumiendo 3 slots
            {
                if (_saveGameRepository.HasSavedProgress(i))
                {
                    int level = _saveGameRepository.GetSavedLevel(i);
                    _view.DisplaySlotInfo(i, $"Nivel: {level}", true);
                }
                else
                {
                    _view.DisplaySlotInfo(i, "Vacío", false);
                }
            }
        }

        private void HandleSlotSelection(int slot)
        {
            UnityEngine.Debug.Log($"PartiesScreenPresenter: HandleSlotSelection called for slot {slot}");
            UnityEngine.Debug.Log($"PartiesScreenPresenter: Current mode - isNewGameMode: {_isNewGameMode}");
            
            _selectedSlot = slot;
            var result = _slotSelectionUseCase.HandleSlotSelection(slot, _isNewGameMode, _firstLevelBuildIndex);

            UnityEngine.Debug.Log($"PartiesScreenPresenter: SlotSelection result action: {result.Action}");
            
            if (result.Action == SlotAction.ShowOverwriteDialog)
            {
                UnityEngine.Debug.Log($"PartiesScreenPresenter: Showing overwrite dialog with message: {result.Message}");
                _view.ShowOverwriteDialog(result.Message);
            }
            // Para StartGame y ContinueGame, el caso de uso ya maneja la navegación
        }

        private void HandleBackRequest()
        {
            _navigationService.GoBack();
        }

        private void HandleOverwriteConfirmed()
        {
            _view.HideOverwriteDialog();
            _slotSelectionUseCase.ConfirmOverwrite(_selectedSlot, _firstLevelBuildIndex);
            _selectedSlot = -1;
        }

        private void HandleOverwriteCancelled()
        {
            _view.HideOverwriteDialog();
            _selectedSlot = -1;
        }

        private bool HasAnyProgress()
        {
            for (int i = 0; i < 3; i++)
            {
                if (_saveGameRepository.HasSavedProgress(i))
                {
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            if (_view != null)
            {
                _view.OnSlotSelected -= HandleSlotSelection;
                _view.OnBackRequested -= HandleBackRequest;
                _view.OnOverwriteConfirmed -= HandleOverwriteConfirmed;
                _view.OnOverwriteCancelled -= HandleOverwriteCancelled;
            }
        }
    }
}
