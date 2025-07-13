using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public const string LastLevelKey = "LastLevel";

    // --- NUEVO: Soporte para múltiples slots de guardado ---
    private const string SaveSlotKeyPrefix = "SaveSlot_";
    public int CurrentSlot { get; private set; } = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveLevelProgressToSlot(int slot, int buildIndex)
    {
        Debug.Log($"[GameManager] Guardando progreso: slot={slot}, nivel={buildIndex}");
        PlayerPrefs.SetInt(SaveSlotKeyPrefix + slot, buildIndex);
        PlayerPrefs.Save();
        CurrentSlot = slot;
    }

    public int GetSavedLevelFromSlot(int slot)
    {
        int value = PlayerPrefs.GetInt(SaveSlotKeyPrefix + slot, 1);
        Debug.Log($"[GameManager] Leyendo progreso: slot={slot}, nivel={value}");
        return value;
    }

    public bool HasSavedProgressInSlot(int slot)
    {
        bool exists = PlayerPrefs.HasKey(SaveSlotKeyPrefix + slot);
        Debug.Log($"[GameManager] ¿Existe progreso en slot {slot}? {exists}");
        return exists;
    }

    public void SetCurrentSlot(int slot)
    {
        CurrentSlot = slot;
    }

    public int GetCurrentSlot()
    {
        return CurrentSlot;
    }

    // Métodos antiguos para compatibilidad (usarán el slot actual si está definido)
    public void SaveLevelProgress(int buildIndex)
    {
        // Parche: Si CurrentSlot no está seteado, intentar leer el último slot usado de PlayerPrefs
        if (CurrentSlot < 0)
        {
            // Buscar el último slot con progreso guardado
            for (int i = 0; i < 3; i++)
            {
                if (HasSavedProgressInSlot(i))
                {
                    SetCurrentSlot(i);
                    break;
                }
            }
        }
        if (CurrentSlot >= 0)
            SaveLevelProgressToSlot(CurrentSlot, buildIndex);
        else
            PlayerPrefs.SetInt(LastLevelKey, buildIndex);
        PlayerPrefs.Save();
    }

    public int GetSavedLevel()
    {
        if (CurrentSlot >= 0)
            return GetSavedLevelFromSlot(CurrentSlot);
        return PlayerPrefs.GetInt(LastLevelKey, 1);
    }

    public bool HasSavedProgress()
    {
        if (CurrentSlot >= 0)
            return HasSavedProgressInSlot(CurrentSlot);
        return PlayerPrefs.HasKey(LastLevelKey);
    }
}
