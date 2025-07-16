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
    public InputField newUsernameInput;
    public Text messageText;

    private UserDataService userService;

    void Start()
    {
        usernameInput.text = SessionManager.CurrentUser;
        newUsernameInput.text = SessionManager.CurrentUser;
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
    public void OnChangeUsernameClicked()
{
    string oldUsername = SessionManager.CurrentUser;
    string newUsername = newUsernameInput.text.Trim();

    if (string.IsNullOrEmpty(newUsername))
    {
        messageText.text = "Debes ingresar un nuevo nombre de usuario.";
        return;
    }
    if (newUsername == oldUsername)
    {
        messageText.text = "El nuevo nombre debe ser diferente al actual.";
        return;
    }
    // Verificar que no existe ya ese nombre
    if (userService.GetUser(newUsername) != null)
    {
        messageText.text = "Ese nombre de usuario ya existe. Elige otro.";
        return;
    }

    // Solo cambia el nombre, nada más
    if (userService.ChangeUserName(oldUsername, newUsername))
    {
        SessionManager.setNameUser(newUsername); // Este método actualiza la variable actual
        messageText.text = "Nombre de usuario cambiado correctamente.";
        usernameInput.text = newUsername;
    }
    else
    {
        messageText.text = "Error al guardar el nuevo nombre de usuario.";
    }
}
    public void onClickMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }
}