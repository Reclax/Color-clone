using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ColorClone.Domain.Interfaces;

namespace ColorClone.Presentation.Unity
{
    /// <summary>
    /// Vista Unity que implementa la selección de slot SOLO para Continuar Juego, con lógica incluida.
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

        void Start()
        {
            InitializeView();
            SetupEventListeners();
        }

        void OnDestroy() { } // Si necesitas limpiar eventos, hazlo aquí

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
                int slotIndex = i; // Necesario para el closure!
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
            // Aquí va la lógica para continuar partida.
            bool hasProgress = SlotHasProgress(slot);
            if (hasProgress)
            {
                ContinueGame(slot);
            }
            else
            {
                // Puedes mostrar un mensaje de error o ignorar el click
                Debug.LogWarning($"No hay partida guardada en el slot {slot}.");
            }
        }

        private bool SlotHasProgress(int slot)
        {
            // Aquí deberías consultar tu repositorio/servicio real
            // Ejemplo: return saveGameRepository.HasSavedProgress(slot);
            return false; // Cambia esto por tu lógica real
        }

        private void ContinueGame(int slot)
        {
            // Aquí tu lógica para cargar el progreso y cambiar de escena
            Debug.Log($"Continuando partida en slot {slot}");
            // Ejemplo: int savedLevel = saveGameRepository.GetSavedLevel(slot);
            // Ejemplo: SceneManager.LoadScene(savedLevel);
        }

        private void UpdateSlotsDisplay()
        {
            for (int i = 0; i < slotButtons.Length; i++)
            {
                // Muestra info de slots según tu lógica
                bool hasProgress = SlotHasProgress(i);
                string slotText = hasProgress ? "Ocupado" : "Vacío";
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