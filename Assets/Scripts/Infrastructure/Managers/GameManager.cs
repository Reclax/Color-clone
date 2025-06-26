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
        PlayerPrefs.SetInt(SaveSlotKeyPrefix + slot, buildIndex);
        PlayerPrefs.Save();
        CurrentSlot = slot;
    }

    public int GetSavedLevelFromSlot(int slot)
    {
        return PlayerPrefs.GetInt(SaveSlotKeyPrefix + slot, 1); // 1 por defecto
    }

    public bool HasSavedProgressInSlot(int slot)
    {
        return PlayerPrefs.HasKey(SaveSlotKeyPrefix + slot);
    }

    public void SetCurrentSlot(int slot)
    {
        CurrentSlot = slot;
    }

    // Métodos antiguos para compatibilidad (usarán el slot actual si está definido)
    public void SaveLevelProgress(int buildIndex)
    {
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
