using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void LoadMarkerless()
    {
        SceneManager.LoadScene("Marker-Less");
    }

    public void LoadMarkerBased()
    {
        SceneManager.LoadScene("Marker-Based");
    }
}