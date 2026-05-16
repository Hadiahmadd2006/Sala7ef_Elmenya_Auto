using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void LoadMarkerlessScene()
    {
        SceneManager.LoadScene("Marker-Less");
    }

    public void LoadMarkerBasedScene()
    {
        SceneManager.LoadScene("Marker-Based");
    }
}