using ColorClone.Domain.Interfaces;

namespace ColorClone.Application.UseCases
{
    /// <summary>
    /// Caso de uso para manejar la selección de slots de partida
    /// Encapsula la lógica de negocio siguiendo Clean Architecture
    /// </summary>
    public class SlotSelectionUseCase
    {
        private readonly ISaveGameRepository _saveGameRepository;
        private readonly ISceneNavigationService _navigationService;

        public SlotSelectionUseCase(
            ISaveGameRepository saveGameRepository, 
            ISceneNavigationService navigationService)
        {
            _saveGameRepository = saveGameRepository;
            _navigationService = navigationService;
        }

        public SlotSelectionResult HandleSlotSelection(int slot, bool isNewGameMode, int firstLevelIndex)
        {
            if (isNewGameMode)
            {
                return HandleNewGameSelection(slot, firstLevelIndex);
            }
            else
            {
                return HandleContinueGameSelection(slot, firstLevelIndex);
            }
        }

        private SlotSelectionResult HandleNewGameSelection(int slot, int firstLevelIndex)
        {
            if (_saveGameRepository.HasSavedProgress(slot))
            {
                return new SlotSelectionResult
                {
                    Action = SlotAction.ShowOverwriteDialog,
                    Message = $"¿Sobrescribir la partida en la casilla {slot + 1}?"
                };
            }
            else
            {
                StartNewGame(slot, firstLevelIndex);
                return new SlotSelectionResult { Action = SlotAction.StartGame };
            }
        }

        private SlotSelectionResult HandleContinueGameSelection(int slot, int firstLevelIndex)
        {
            if (_saveGameRepository.HasSavedProgress(slot))
            {
                ContinueGame(slot);
                return new SlotSelectionResult { Action = SlotAction.ContinueGame };
            }
            else
            {
                StartNewGame(slot, firstLevelIndex);
                return new SlotSelectionResult { Action = SlotAction.StartGame };
            }
        }

        public void ConfirmOverwrite(int slot, int firstLevelIndex)
        {
            StartNewGame(slot, firstLevelIndex);
        }

        private void StartNewGame(int slot, int firstLevelIndex)
        {
            _saveGameRepository.SetCurrentSlot(slot);
            _saveGameRepository.SaveProgress(slot, firstLevelIndex);
            _navigationService.NavigateToScene(firstLevelIndex);
        }

        private void ContinueGame(int slot)
        {
            _saveGameRepository.SetCurrentSlot(slot);
            int savedLevel = _saveGameRepository.GetSavedLevel(slot);
            _navigationService.NavigateToScene(savedLevel);
        }
    }

    public class SlotSelectionResult
    {
        public SlotAction Action { get; set; }
        public string Message { get; set; }
    }

    public enum SlotAction
    {
        StartGame,
        ContinueGame,
        ShowOverwriteDialog
    }
}
