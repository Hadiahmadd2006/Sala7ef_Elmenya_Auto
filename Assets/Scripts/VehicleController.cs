using UnityEngine;
public class VehicleController : MonoBehaviour
{
    [Header("Body")]
    [SerializeField] private Renderer bodyRenderer; [SerializeField]
    private Material[]
    bodyMaterials; private int currentBodyIndex; [Header("Wheels")]
    [SerializeField]
    private Renderer[] wheelRenderers; [SerializeField] private Material[] wheelMaterials;
    [SerializeField] private WheelSpinner[] wheels; private int currentWheelIndex;
    [Header("Audio")][SerializeField] private AudioSource engineSource; [SerializeField]
    private AudioSource voiceoverSource; [SerializeField] private AudioClip engineStartClip;
    [SerializeField] private AudioClip engineLoopClip; private bool engineOn;
    [Header("Doors")][SerializeField] private DoorPart[] doors; public void
    NextBodyColor()
    {
        currentBodyIndex = (currentBodyIndex + 1) %
    bodyMaterials.Length; bodyRenderer.material = bodyMaterials[currentBodyIndex];
        Save();
    }
    public void NextWheelStyle()
    {
        currentWheelIndex =
        (currentWheelIndex + 1) % wheelMaterials.Length; foreach (var r in wheelRenderers)
            r.material = wheelMaterials[currentWheelIndex]; Save();
    }
    public void
            ToggleEngine()
    {
        engineOn = !engineOn; if (engineOn)
        {
            engineSource.PlayOneShot(engineStartClip); engineSource.clip = engineLoopClip;
            engineSource.loop = true; engineSource.PlayDelayed(engineStartClip.length);
        }
        else { engineSource.Stop(); }
        foreach (var w in wheels) w.Spinning =
        engineOn;
    }
    public void PlayVoiceover()
    {
        if (!voiceoverSource.isPlaying)
            voiceoverSource.Play();
    }
    public void ResetVehicle()
    {
        currentBodyIndex =
            currentWheelIndex = 0; bodyRenderer.material = bodyMaterials[0]; foreach (var
            r in wheelRenderers) r.material = wheelMaterials[0]; if (engineOn) ToggleEngine();
        transform.localScale = Vector3.one; Save();
    }
    void Start()
    {
        currentBodyIndex = PlayerPrefs.GetInt("BodyIdx_" + name, 0); currentWheelIndex =
        PlayerPrefs.GetInt("WheelIdx_" + name, 0); bodyRenderer.material =
        bodyMaterials[currentBodyIndex]; foreach (var r in wheelRenderers) r.material =
        wheelMaterials[currentWheelIndex];
    }
    void Save()
    {
        PlayerPrefs.SetInt("BodyIdx_" + name, currentBodyIndex);
        PlayerPrefs.SetInt("WheelIdx_" + name, currentWheelIndex);
    }
}