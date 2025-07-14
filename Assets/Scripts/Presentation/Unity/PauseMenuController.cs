using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ColorClone.Domain.Interfaces;
using Zenject;
using Assets.Scripts.Infrastructure.Managers;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;
    public Button btnStartScreen;
    public Button btnExit;

    [Inject] private IGameFlowService _gameFlowService;
    [Inject] private IInputService _inputService;

    [Header("Opcional: nombre de la escena de inicio")]
    public string startScreenName = "StartScreen";

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (btnStartScreen != null)
            btnStartScreen.onClick.AddListener(ResumeGame);

        if (btnExit != null)
            btnExit.onClick.AddListener(ReturnToStartScreen);
    }

    void Update()
    {
        if (_inputService != null && _inputService.GetPauseDown())
        {
            if (pausePanel != null)
            {
                bool show = !pausePanel.activeSelf;
                pausePanel.SetActive(show);
                Time.timeScale = show ? 0f : 1f;
            }
        }
    }

    private void ResumeGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ReturnToStartScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(startScreenName);
    }
}
