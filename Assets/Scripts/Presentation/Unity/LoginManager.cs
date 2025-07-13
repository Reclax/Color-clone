using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
    public Button enterGameButton;
    public Button addPasswordButton;
    public Text messageText;

    private const string DefaultUser = "admin";
    private const string DefaultPass = "admin";
    private const string ExtraPassKey = "ExtraPassword";
    private bool isLoggedIn = false;

    void Start()
    {
        enterGameButton.interactable = false;
        addPasswordButton.interactable = false;
        loginButton.onClick.AddListener(OnLoginClicked);
        enterGameButton.onClick.AddListener(OnEnterGameClicked);
        addPasswordButton.onClick.AddListener(OnAddPasswordClicked);
    }

    void OnLoginClicked()
    {
        string user = usernameInput.text;
        string pass = passwordInput.text;
        string extraPass = PlayerPrefs.GetString(ExtraPassKey, "");

        if ((user == DefaultUser && pass == DefaultPass) || (user == DefaultUser && pass == extraPass && extraPass != ""))
        {
            isLoggedIn = true;
            messageText.text = "Inicio de sesión exitoso.";
            enterGameButton.interactable = true;
            addPasswordButton.interactable = true;
        }
        else
        {
            messageText.text = "Usuario o contraseña incorrectos.";
        }
    }

    void OnEnterGameClicked()
    {
        if (isLoggedIn)
        {
            SceneManager.LoadScene("StartScreen");
        }
    }

    void OnAddPasswordClicked()
    {
        if (isLoggedIn)
        {
            string newPass = passwordInput.text;
            if (!string.IsNullOrEmpty(newPass) && newPass != DefaultPass)
            {
                PlayerPrefs.SetString(ExtraPassKey, newPass);
                PlayerPrefs.Save();
                messageText.text = "Contraseña adicional guardada.";
            }
            else
            {
                messageText.text = "La nueva contraseña no puede ser vacía ni 'admin'.";
            }
        }
    }
}
