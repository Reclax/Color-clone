using UnityEngine;
using UnityEngine.UI;
using Services;
using ColorClone.Presentation.Unity;
using UnityEngine.SceneManagement;
using Assets.Scripts.Infrastructure.Managers;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;

    private UserDataService userService;

    void Start()
    {
        userService = new UserDataService();
    }

    public void OnLoginClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        if (userService.ValidatePassword(username, password)) {

        SessionManager.Login(username);
        SceneManager.LoadScene("StartScreen"); }
        
        else
            messageText.text = "Usuario o contraseña incorrectos";
    }

    public void OnRegisterClicked()
    {
        SceneManager.LoadScene("RegisterScene");
    }
}