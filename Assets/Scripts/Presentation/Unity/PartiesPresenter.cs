using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ColorClone.Domain.Interfaces;
using ColorClone.Application.Presenters;
using ColorClone.Application.UseCases;
using ColorClone.Infrastructure.Adapters;
using ColorClone.Infrastructure.Services;
using Zenject;

namespace ColorClone.Presentation.Unity
{
    /// <summary>
    /// Vista Unity que implementa IPartiesView siguiendo el patrón MVP
    /// Solo maneja la UI, toda la lógica está en el Presenter
    /// Aplicando Clean Architecture y principios SOLID
    /// </summary>
    public class PartiesPresenter : MonoBehaviour, IPartiesView
    {
        #region Events from IPartiesView
        public event Action<int> OnSlotSelected;
        public event Action OnBackRequested;
        public event Action OnOverwriteConfirmed;
        public event Action OnOverwriteCancelled;
        #endregion

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
        public int firstLevelBuildIndex = 1;
        public string startScreenName = "StartScreen";

        // Presenter siguiendo Clean Architecture
        [Inject]
        private PartiesScreenPresenter _presenter;

        // Variable estática para comunicación entre escenas
        private static bool _globalNewGameMode = false;

        public static void SetGlobalNewGameMode(bool newGameMode)
        {
            _globalNewGameMode = newGameMode;
        }

        void Start()
        {
            // Asegurar que el diálogo de sobrescritura esté oculto al inicio
            if (overwriteDialog != null)
            {
                overwriteDialog.SetActive(false);
            }
            // Conectar la vista con el presenter inyectado
            _presenter.SetView(this);
            _presenter.Initialize(_globalNewGameMode, firstLevelBuildIndex);
            SetupEventListeners();
        }

        void OnDestroy()
        {
            _presenter?.Dispose();
        }

        #region Architecture Setup
        // El método InitializeArchitecture ya no es necesario gracias a Zenject
        #endregion

        #region UI Event Setup
        private void SetupEventListeners()
        {
            SetupButtons(slotButtons, OnSlotSelected);
            SetupButton(confirmOverwriteButton, () => OnOverwriteConfirmed?.Invoke());
            SetupButton(cancelOverwriteButton, () => OnOverwriteCancelled?.Invoke());
            SetupButton(backButton, () => OnBackRequested?.Invoke());
        }

        private void SetupButtons(Button[] buttons, Action<int> onClick)
        {
            if (buttons == null) return;
            for (int i = 0; i < buttons.Length; i++)
            {
                SetupButton(buttons[i], () => onClick?.Invoke(i));
            }
        }

        private void SetupButton(Button button, Action onClick)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());
        }
        #endregion

        #region IPartiesView Implementation
        public void DisplayTitle(string title)
        {
            if (titleText != null)
            {
                titleText.text = title;
            }
        }

        public void DisplaySlotInfo(int slotIndex, string text, bool hasProgress)
        {
            SetSlotText(slotIndex, text);
        }

        private void SetSlotText(int slotIndex, string text)
        {
            if (slotLabels != null && slotIndex < slotLabels.Length && slotLabels[slotIndex] != null)
            {
                slotLabels[slotIndex].text = text;
                return;
            }
            if (slotButtons != null && slotIndex < slotButtons.Length && slotButtons[slotIndex] != null)
            {
                var textComponent = slotButtons[slotIndex].GetComponentInChildren<Text>();
                var tmpComponent = slotButtons[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = text;
                }
                else if (tmpComponent != null)
                {
                    tmpComponent.text = text;
                }
            }
        }

        public void ShowOverwriteDialog(string message)
        {
            Debug.Log($"PartiesPresenter: ShowOverwriteDialog called with message: {message}");
            Debug.LogWarning("DEBUGGING: ¿Por qué se está mostrando el diálogo de sobrescritura?");
            
            if (overwriteDialogText != null)
            {
                overwriteDialogText.text = message;
            }

            if (overwriteDialog != null)
            {
                overwriteDialog.SetActive(true);
                Debug.Log("PartiesPresenter: Overwrite dialog shown");
            }
        }

        public void HideOverwriteDialog()
        {
            Debug.Log("PartiesPresenter: HideOverwriteDialog called");
            
            if (overwriteDialog != null)
            {
                overwriteDialog.SetActive(false);
                Debug.Log("PartiesPresenter: Overwrite dialog hidden");
            }
        }
        #endregion

        #region Public Methods for Inspector (Backward Compatibility)
        // Estos métodos mantienen compatibilidad con la configuración anterior
        public void OnSlot0ButtonClick() => OnSlotSelected?.Invoke(0);
        public void OnSlot1ButtonClick() => OnSlotSelected?.Invoke(1);
        public void OnSlot2ButtonClick() => OnSlotSelected?.Invoke(2);
        public void OnBackButtonClick() => OnBackRequested?.Invoke();
        public void OnConfirmOverwriteClick() => OnOverwriteConfirmed?.Invoke();
        public void OnCancelOverwriteClick() => OnOverwriteCancelled?.Invoke();
        #endregion
    }
}

