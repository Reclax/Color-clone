using UnityEngine;
using UnityEngine.UI;
using Services;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;

    private UserDataService userService;

    void Start()
    {
        userService = new UserDataService();
    }

    public void OnRegisterClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            messageText.text = "Completa ambos campos.";
            return;
        }

        if (userService.GetUser(username) != null)
        {
            messageText.text = "El usuario ya existe.";
            return;
        }

        User newUser = new User
        {
            username = username,
            password = password,
            oldPasswords = new List<string>(),
            progress = new List<int> { 0, 0, 0 }
        };

        userService.AddUser(newUser);
        messageText.text = "¡Registrado exitosamente!";
    }

    public void OnBackToLoginClicked()
    {
     SceneManager.LoadScene("LoginScene");
    }
}