using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneNavigator : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "mainmenu";
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}