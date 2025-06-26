using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColorClone.Presentation.Unity
{
    /// <summary>
    /// Maneja la navegación entre escenas de UI
    /// </summary>
    public class SceneNavigator : MonoBehaviour
    {
        [Header("Scene Names")]
        public string startScreenScene = "StartScreen";
        public string partiesScene = "Parties";
        
        public void GoToStartScreen()
        {
            SceneManager.LoadScene(startScreenScene);
        }
        
        public void GoToPartiesScreen()
        {
            SceneManager.LoadScene(partiesScene);
        }
        
        public void QuitApplication()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                UnityEngine.Application.Quit();
            #endif
        }
    }
}
