using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace ColorClone.Presentation.Unity
{
    public class PartiesPresenter : MonoBehaviour
    {
        [Header("Slots UI")]
        public Button[] slotButtons; // Asignar en el inspector
        public Text[] slotLabels;    // Asignar en el inspector
        public GameObject overwriteDialog;
        public Text overwriteDialogText;
        public Button confirmOverwriteButton;
        public Button cancelOverwriteButton;

        [Header("UI Elements")]
        public Text titleText; // Título que cambia según el modo
        public Button backButton;

        [Header("Configuración")]
        public int firstLevelBuildIndex = 1;
        public string endScreenName = "EndScreen";
        public string startScreenName = "StartScreen";

        // Variable estática para comunicación entre escenas
        private static bool globalNewGameMode = false;

        private int selectedSlot = -1;
        private bool isNewGame = false;

        public static void SetGlobalNewGameMode(bool newGameMode)
        {
            globalNewGameMode = newGameMode;
        }

        void Start()
        {
            Debug.Log("PartiesPresenter Start() called");

            // Verificar que GameManager existe
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null! Make sure GameManager is in the scene.");
                return;
            }

            // Obtener modo desde variable estática
            isNewGame = globalNewGameMode;
            Debug.Log($"IsNewGame mode: {isNewGame}");

            // Si no hay partidas guardadas, forzar modo nueva partida
            if (!isNewGame)
            {
                DetectGameMode();
            }

            UpdateUI();
            SetupEventListeners();
        }

        void DetectGameMode()
        {
            // Si no hay ninguna partida guardada, forzar modo nueva partida
            bool hasAnyProgress = false;
            for (int i = 0; i < 3; i++) // Assuming 3 slots
            {
                if (GameManager.Instance != null && GameManager.Instance.HasSavedProgressInSlot(i))
                {
                    hasAnyProgress = true;
                    break;
                }
            }

            // Si no hay progreso, automáticamente es nueva partida
            if (!hasAnyProgress)
            {
                isNewGame = true;
            }
        }

        void SetupEventListeners()
        {
            Debug.Log("Setting up event listeners...");

            // Configurar slots
            UpdateSlotsUI();

            if (slotButtons != null && slotButtons.Length > 0)
            {
                for (int i = 0; i < slotButtons.Length; i++)
                {
                    if (slotButtons[i] != null)
                    {
                        int slot = i;
                        slotButtons[i].onClick.RemoveAllListeners(); // Limpiar listeners anteriores
                        slotButtons[i].onClick.AddListener(() => {
                            Debug.Log($"Button {slot} clicked via code listener!");
                            OnSlotSelected(slot);
                        });
                        Debug.Log($"Added listener to slot {i}");
                        
                        // Verificar si el botón es interactuable
                        if (!slotButtons[i].interactable)
                        {
                            Debug.LogWarning($"Slot button {i} is not interactable!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Slot button {i} is null!");
                    }
                }
            }
            else
            {
                Debug.LogError("SlotButtons array is null or empty!");
            }

            // Configurar diálogo de sobrescritura
            if (overwriteDialog != null)
            {
                overwriteDialog.SetActive(false);

                if (confirmOverwriteButton != null)
                {
                    confirmOverwriteButton.onClick.RemoveAllListeners();
                    confirmOverwriteButton.onClick.AddListener(OnConfirmOverwrite);
                }
                else
                {
                    Debug.LogWarning("ConfirmOverwriteButton is null!");
                }

                if (cancelOverwriteButton != null)
                {
                    cancelOverwriteButton.onClick.RemoveAllListeners();
                    cancelOverwriteButton.onClick.AddListener(OnCancelOverwrite);
                }
                else
                {
                    Debug.LogWarning("CancelOverwriteButton is null!");
                }
            }
            else
            {
                Debug.LogWarning("OverwriteDialog is null!");
            }

            // Configurar botón de regreso
            if (backButton != null)
            {
                backButton.onClick.RemoveAllListeners();
                backButton.onClick.AddListener(GoBackToStartScreen);
                Debug.Log("Back button listener added");
            }
            else
            {
                Debug.LogWarning("BackButton is null!");
            }
        }

        void UpdateUI()
        {
            if (titleText != null)
            {
                titleText.text = isNewGame ? "Nueva Partida - Seleccionar Slot" : "Continuar Partida - Seleccionar Slot";
            }
        }

        public void GoBackToStartScreen()
        {
            SceneManager.LoadScene(startScreenName);
        }

        public void SetNewGameMode(bool newGame)
        {
            isNewGame = newGame;
        }

        void UpdateSlotsUI()
        {
            Debug.Log("Updating slots UI...");

            // Si slotLabels no está configurado, intentar encontrar automáticamente
            if (slotLabels == null || slotLabels.Length == 0)
            {
                Debug.LogWarning("SlotLabels array is null or empty! Attempting to find them automatically...");
                TryAutoFindSlotLabels();
            }

            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager.Instance is null!");
                return;
            }

            // Si aún no tenemos labels después del intento automático, usar los botones
            if (slotLabels == null || slotLabels.Length == 0)
            {
                Debug.LogWarning("Could not find slot labels, will update button texts instead");
                UpdateButtonTexts();
                return;
            }

            for (int i = 0; i < slotLabels.Length; i++)
            {
                if (slotLabels[i] != null)
                {
                    if (GameManager.Instance.HasSavedProgressInSlot(i))
                    {
                        int lvl = GameManager.Instance.GetSavedLevelFromSlot(i);
                        slotLabels[i].text = $"Nivel: {lvl}";
                        Debug.Log($"Slot {i}: Level {lvl}");
                    }
                    else
                    {
                        slotLabels[i].text = "Vacío";
                        Debug.Log($"Slot {i}: Empty");
                    }
                }
                else
                {
                    Debug.LogWarning($"SlotLabel {i} is null!");
                }
            }
        }

        void TryAutoFindSlotLabels()
        {
            if (slotButtons != null && slotButtons.Length > 0)
            {
                slotLabels = new Text[slotButtons.Length];
                for (int i = 0; i < slotButtons.Length; i++)
                {
                    if (slotButtons[i] != null)
                    {
                        // Buscar Text component en el botón o sus hijos
                        Text textComponent = slotButtons[i].GetComponentInChildren<Text>();
                        if (textComponent != null)
                        {
                            slotLabels[i] = textComponent;
                            Debug.Log($"Auto-found Text component for slot {i}");
                        }
                        else
                        {
                            // Si no hay Text, buscar TextMeshPro
                            TextMeshProUGUI tmpComponent = slotButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                            if (tmpComponent != null)
                            {
                                Debug.Log($"Found TextMeshPro component for slot {i}, but using fallback method");
                            }
                            Debug.LogWarning($"Could not find Text component for slot {i}");
                        }
                    }
                }
            }
        }

        void UpdateButtonTexts()
        {
            if (slotButtons == null) return;

            for (int i = 0; i < slotButtons.Length; i++)
            {
                if (slotButtons[i] != null)
                {
                    // Intentar Text primero
                    Text buttonText = slotButtons[i].GetComponentInChildren<Text>();
                    TextMeshProUGUI buttonTMP = slotButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    
                    string textToSet;
                    if (GameManager.Instance.HasSavedProgressInSlot(i))
                    {
                        int lvl = GameManager.Instance.GetSavedLevelFromSlot(i);
                        textToSet = $"Slot {i + 1}\nNivel: {lvl}";
                    }
                    else
                    {
                        textToSet = $"Slot {i + 1}\nVacío";
                    }

                    if (buttonText != null)
                    {
                        buttonText.text = textToSet;
                        Debug.Log($"Updated Text for slot {i}: {textToSet}");
                    }
                    else if (buttonTMP != null)
                    {
                        buttonTMP.text = textToSet;
                        Debug.Log($"Updated TextMeshPro for slot {i}: {textToSet}");
                    }
                    else
                    {
                        Debug.LogWarning($"No text component found for slot {i}");
                    }
                }
            }
        }

        void OnSlotSelected(int slot)
        {
            Debug.Log($"Slot {slot} selected!");
            selectedSlot = slot;

            if (isNewGame)
            {
                Debug.Log("New game mode");
                if (GameManager.Instance.HasSavedProgressInSlot(slot))
                {
                    Debug.Log($"Slot {slot} has saved progress, showing overwrite dialog");
                    // Mostrar diálogo de sobrescritura
                    if (overwriteDialogText != null)
                        overwriteDialogText.text = $"¿Sobrescribir la partida en la casilla {slot + 1}?";
                    if (overwriteDialog != null)
                        overwriteDialog.SetActive(true);
                }
                else
                {
                    Debug.Log($"Slot {slot} is empty, starting new game");
                    StartNewGame(slot);
                }
            }
            else // Continuar
            {
                Debug.Log("Continue mode");
                if (GameManager.Instance.HasSavedProgressInSlot(slot))
                {
                    Debug.Log($"Continuing game from slot {slot}");
                    ContinueGame(slot);
                }
                else
                {
                    Debug.Log($"Slot {slot} is empty, starting new game instead");
                    // Slot vacío → nueva partida
                    StartNewGame(slot);
                }
            }
        }

        void StartNewGame(int slot)
        {
            GameManager.Instance.SetCurrentSlot(slot);
            GameManager.Instance.SaveLevelProgressToSlot(slot, firstLevelBuildIndex);
            SceneManager.LoadScene(firstLevelBuildIndex);
        }

        void ContinueGame(int slot)
        {
            GameManager.Instance.SetCurrentSlot(slot);
            int lvl = GameManager.Instance.GetSavedLevelFromSlot(slot);
            SceneManager.LoadScene(lvl);
        }

        void OnConfirmOverwrite()
        {
            overwriteDialog.SetActive(false);
            StartNewGame(selectedSlot);
        }

        void OnCancelOverwrite()
        {
            overwriteDialog.SetActive(false);
            selectedSlot = -1;
        }

        // Métodos públicos para ser asignados directamente en el Inspector de Unity
        // Estos pueden ser usados como respaldo si los listeners por código no funcionan
        public void OnSlot0ButtonClick()
        {
            Debug.Log("Slot 0 clicked via Inspector!");
            OnSlotSelected(0);
        }

        public void OnSlot1ButtonClick()
        {
            Debug.Log("Slot 1 clicked via Inspector!");
            OnSlotSelected(1);
        }

        public void OnSlot2ButtonClick()
        {
            Debug.Log("Slot 2 clicked via Inspector!");
            OnSlotSelected(2);
        }

        public void OnBackButtonClick()
        {
            Debug.Log("Back button clicked via Inspector!");
            GoBackToStartScreen();
        }

        public void OnConfirmOverwriteClick()
        {
            Debug.Log("Confirm overwrite clicked via Inspector!");
            OnConfirmOverwrite();
        }

        public void OnCancelOverwriteClick()
        {
            Debug.Log("Cancel overwrite clicked via Inspector!");
            OnCancelOverwrite();
        }
    }
}

