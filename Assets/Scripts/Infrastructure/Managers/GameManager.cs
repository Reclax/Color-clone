using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public const string LastLevelKey = "LastLevel";

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

    public void SaveLevelProgress(int buildIndex)
    {
        PlayerPrefs.SetInt(LastLevelKey, buildIndex);
        PlayerPrefs.Save();
    }

    public int GetSavedLevel()
    {
        return PlayerPrefs.GetInt(LastLevelKey, 1); // 1 por defecto
    }

    public bool HasSavedProgress()
    {
        return PlayerPrefs.HasKey(LastLevelKey);
    }
}

