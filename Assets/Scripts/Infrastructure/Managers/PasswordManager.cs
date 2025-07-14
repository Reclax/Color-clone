using UnityEngine;
using UnityEngine.UI;
using Services;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using Assets.Scripts.Infrastructure.Managers;

public class PasswordManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;

    private UserDataService userService;

    void Start()
    {
        usernameInput.text = SessionManager.CurrentUser;
        usernameInput.enabled = false;
        userService = new UserDataService();
    }

    public void OnChangePasswordClicked()
    {
        
        string username = usernameInput.text;
        string newPassword = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword))
        {
            messageText.text = "Completa ambos campos.";
            return;
        }

        if (userService.ChangePassword(username, newPassword))
            messageText.text = "Contraseña cambiada correctamente.";
        else
            messageText.text = "No se pudo cambiar la contraseña. Revisa que sea diferente y no usada antes.";
    }
    public void onClickMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }
}