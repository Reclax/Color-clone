using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ColorClone.Domain.Interfaces;
using Assets.Scripts.Infrastructure.Managers;
using Services; // Para UserDataService
using UnityEngine.SceneManagement;

namespace ColorClone.Presentation.Unity
{
    /// <summary>
    /// Vista Unity para selección de slot SOLO para Nuevo Juego. 
    /// Usa el usuario actual de SessionManager, no requiere parámetros externos.
    /// </summary>
    public class PartiesNewGamePresenter : MonoBehaviour, INewGamePartiesView
    {
        [Header("Slots UI")]
        public Button[] slotButtons;
        public Text[] slotLabels;
        public GameObject overwriteDialog;
        public Text overwriteDialogText;
        public Button confirmOverwriteButton;
        public Button cancelOverwriteButton;

        [Header("UI Elements")]
        public Text titleText;
        public Button backButton;

        [Header("Configuración")]
        public int firstLevelBuildIndex = 1; // Primer nivel para nueva partida
        public string gameSceneName = "Level1"; // Cambia esto si tu primer nivel es otro

        private int _selectedSlot = -1;
        private UserDataService _userDataService;
        private User _currentUser;

        void Start()
        {
            if (overwriteDialog != null) overwriteDialog.SetActive(false);

            _userDataService = new UserDataService();
            // Carga el usuario actual desde SessionManager (NO recibe parámetros)
            string username = SessionManager.CurrentUser;
            _currentUser = _userDataService.GetUser(username);

            InitializeView();
            SetupEventListeners();
        }

        private void InitializeView()
        {
            DisplayTitle("Nueva Partida - Seleccionar Slot");
            UpdateSlotsDisplay();
        }

        private void SetupEventListeners()
        {
            SetupButtons(slotButtons, SlotButtonClicked);
            SetupButton(confirmOverwriteButton, OnConfirmOverwriteClick);
            SetupButton(cancelOverwriteButton, OnCancelOverwriteClick);
        }

        private void SetupButtons(Button[] buttons, Action<int> onClick)
        {
            if (buttons == null) return;
            for (int i = 0; i < buttons.Length; i++)
            {
                int slotIndex = i;
                SetupButton(buttons[i], () => onClick?.Invoke(slotIndex));
            }
        }

        private void SetupButton(Button button, Action onClick)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());
        }

        private void SlotButtonClicked(int slot)
        {
            _selectedSlot = slot;
            bool hasProgress = SlotHasProgress(slot);
            if (hasProgress && overwriteDialog != null)
            {
                ShowOverwriteDialog("¿Sobrescribir la partida?");
            }
            else
            {
                StartNewGame(slot);
            }
        }

        // Verifica si el slot del usuario actual tiene progreso (nivel > 0)
        private bool SlotHasProgress(int slot)
        {
            if (_currentUser == null || _currentUser.progress == null) return false;
            if (slot < 0 || slot >= _currentUser.progress.Count) return false;
            return _currentUser.progress[slot] > 0;
        }

        // Inicia una nueva partida en el slot seleccionado, y guarda el progreso inicial
        private void StartNewGame(int slot)
        {
            if (_currentUser == null) return;

            // Guardar progreso: pon el primer nivel en el slot elegido
            _currentUser.progress[slot] = firstLevelBuildIndex;
            _userDataService.UpdateUser(_currentUser);

            Debug.Log($"Iniciando nueva partida en slot {slot}, nivel {firstLevelBuildIndex}");

            // Cargar la escena del primer nivel (ajusta el nombre si es necesario)
            SceneManager.LoadScene(gameSceneName);
        }

        private void UpdateSlotsDisplay()
        {
            for (int i = 0; i < slotButtons.Length; i++)
            {
                bool hasProgress = SlotHasProgress(i);
                string slotText = hasProgress ? $"Ocupado (Nivel {_currentUser.progress[i]})" : "Vacío";
                DisplaySlotInfo(i, slotText, hasProgress);
            }
        }

        public void DisplayTitle(string title)
        {
            if (titleText != null) titleText.text = title;
        }

        public void DisplaySlotInfo(int slotIndex, string text, bool hasProgress) => SetSlotText(slotIndex, text);

        private void SetSlotText(int slotIndex, string text)
        {
            if (slotLabels != null && slotIndex < slotLabels.Length && slotLabels[slotIndex] != null)
                slotLabels[slotIndex].text = text;
            else if (slotButtons != null && slotIndex < slotButtons.Length && slotButtons[slotIndex] != null)
            {
                var textComponent = slotButtons[slotIndex].GetComponentInChildren<Text>();
                var tmpComponent = slotButtons[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null) textComponent.text = text;
                else if (tmpComponent != null) tmpComponent.text = text;
            }
        }

        public void ShowOverwriteDialog(string message)
        {
            if (overwriteDialogText != null) overwriteDialogText.text = message;
            if (overwriteDialog != null) overwriteDialog.SetActive(true);
        }

        public void HideOverwriteDialog()
        {
            if (overwriteDialog != null) overwriteDialog.SetActive(false);
        }

        // Métodos públicos para Inspector (si los usas)
        public void OnSlot0ButtonClick() => SlotButtonClicked(0);
        public void OnSlot1ButtonClick() => SlotButtonClicked(1);
        public void OnSlot2ButtonClick() => SlotButtonClicked(2);

        public void OnBackButtonClick()
        {
            // Aquí puedes implementa la lógica para volver al menú principal
            SceneManager.LoadScene("StartScreen"); 
        }
        public void OnConfirmOverwriteClick()
        {
            HideOverwriteDialog();
            StartNewGame(_selectedSlot);
        }
        public void OnCancelOverwriteClick()
        {
            HideOverwriteDialog();
            _selectedSlot = -1;
        }
    }
}