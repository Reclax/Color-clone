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
    /// Vista Unity que implementa la selección de slot SOLO para Continuar Juego, con lógica incluida.
    /// Usa el usuario actual de SessionManager, no requiere parámetros externos.
    /// </summary>
    public class PartiesContinueGamePresenter : MonoBehaviour, IContinueGamePartiesView
    {
        public event Action OnBackRequested;

        [Header("Slots UI")]
        public Button[] slotButtons;
        public Text[] slotLabels;

        [Header("UI Elements")]
        public Text titleText;
        public Button backButton;

        [Header("Configuración")]
        public string startScreenName = "StartScreen";
        public string gameSceneNamePrefix = "Level"; // Ejemplo: "Level" + n para el nombre de la escena

        private UserDataService _userDataService;
        private User _currentUser;

        void Start()
        {
            _userDataService = new UserDataService();
            string username = SessionManager.CurrentUser;
            _currentUser = _userDataService.GetUser(username);

            InitializeView();
            SetupEventListeners();
        }

        private void InitializeView()
        {
            DisplayTitle("Continuar Partida - Seleccionar Slot");
            UpdateSlotsDisplay();
        }

        private void SetupEventListeners()
        {
            SetupButtons(slotButtons, SlotButtonClicked);
            SetupButton(backButton, OnBackButtonClick);
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
            bool hasProgress = SlotHasProgress(slot);
            if (hasProgress)
            {
                ContinueGame(slot);
            }
            else
            {
                Debug.LogWarning($"No hay partida guardada en el slot {slot}.");
            }
        }

        // Verifica si el slot del usuario actual tiene progreso (nivel > 0)
        private bool SlotHasProgress(int slot)
        {
            if (_currentUser == null || _currentUser.progress == null) return false;
            if (slot < 0 || slot >= _currentUser.progress.Count) return false;
            return _currentUser.progress[slot] > 0;
        }

        // Carga la partida del slot seleccionado y cambia a la escena correspondiente
        private void ContinueGame(int slot)
        {
            if (_currentUser == null) return;

            int savedLevel = _currentUser.progress[slot];

            Debug.Log($"Continuando partida en slot {slot}, nivel {savedLevel}");

            // Cargar la escena del nivel guardado
            string sceneName = gameSceneNamePrefix + savedLevel.ToString();
            SceneManager.LoadScene(sceneName);
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

        // Métodos públicos para Inspector (si los usas)
        public void OnSlot0ButtonClick() => SlotButtonClicked(0);
        public void OnSlot1ButtonClick() => SlotButtonClicked(1);
        public void OnSlot2ButtonClick() => SlotButtonClicked(2);
        public void OnBackButtonClick() => OnBackRequested?.Invoke();
    }
}