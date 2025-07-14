using Assets.Scripts.Infrastructure.Managers;
using Services;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int CurrentSlot { get; private set; } = -1;
    private ProgressService progressService;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        progressService = new ProgressService(new UserDataService());
    }

    public void SaveLevelProgressToSlot(int slot, int buildIndex)
    {
        string user = SessionManager.CurrentUser;
        if (!string.IsNullOrEmpty(user))
        {
            progressService.SetProgress(user, slot, buildIndex);
            CurrentSlot = slot;
        }
    }

    public int GetSavedLevelFromSlot(int slot)
    {
        string user = SessionManager.CurrentUser;
        if (!string.IsNullOrEmpty(user))
            return progressService.GetProgress(user, slot);
        return 1;
    }

    public bool HasSavedProgressInSlot(int slot)
    {
        string user = SessionManager.CurrentUser;
        if (!string.IsNullOrEmpty(user))
            return progressService.GetProgress(user, slot) > 0;
        return false;
    }

    public void SetCurrentSlot(int slot)
    {
        CurrentSlot = slot;
    }

    public int GetCurrentSlot()
    {
        return CurrentSlot;
    }

    // Métodos antiguos de compatibilidad
    public void SaveLevelProgress(int buildIndex)
    {
        if (CurrentSlot >= 0)
            SaveLevelProgressToSlot(CurrentSlot, buildIndex);
    }

    public int GetSavedLevel()
    {
        if (CurrentSlot >= 0)
            return GetSavedLevelFromSlot(CurrentSlot);
        return 1;
    }

    public bool HasSavedProgress()
    {
        if (CurrentSlot >= 0)
            return HasSavedProgressInSlot(CurrentSlot);
        return false;
    }
}