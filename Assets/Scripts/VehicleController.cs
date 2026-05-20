using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Body - assign the Body PARENT GameObject")]
    [SerializeField] private GameObject bodyParent;
    [SerializeField] private Material[] bodyMaterials;
    private Renderer[] bodyRenderers;
    private int currentBodyIndex;

    [Header("Wheels - assign the Wheels PARENT GameObject")]
    [SerializeField] private GameObject wheelsParent;
    [SerializeField] private Material[] wheelMaterials;
    private Renderer[] wheelRenderers;
    private int currentWheelIndex;

    [Header("Audio")]
    [SerializeField] private AudioSource engineSource;
    [SerializeField] private AudioSource voiceoverSource;
    [SerializeField] private AudioClip engineStartClip;
    private bool engineOn;

    [Header("Doors - assign the Door GameObjects")]
    [SerializeField] private GameObject[] doors;
    private DoorPart[] doorParts;

    void Awake()
    {
        bodyRenderers = (bodyParent != null)
            ? bodyParent.GetComponentsInChildren<Renderer>(true)
            : new Renderer[0];

        // Wheels: read parent, grab all child renderers
        wheelRenderers = (wheelsParent != null)
            ? wheelsParent.GetComponentsInChildren<Renderer>(true)
            : new Renderer[0];

        // Doors: read each door GameObject, grab its DoorPart component
        if (doors != null)
        {
            doorParts = new DoorPart[doors.Length];
            for (int i = 0; i < doors.Length; i++)
                if (doors[i] != null)
                    doorParts[i] = doors[i].GetComponent<DoorPart>();
        }
        else doorParts = new DoorPart[0];
    }

    public void NextBodyColor()
    {
        if (bodyMaterials.Length == 0 || bodyRenderers.Length == 0) return;
        currentBodyIndex = (currentBodyIndex + 1) % bodyMaterials.Length;
        ApplyBodyMaterial();
        Save();
    }

    void ApplyBodyMaterial()
    {
        if (bodyMaterials.Length == 0 || bodyRenderers.Length == 0) return;
        currentBodyIndex = Mathf.Clamp(currentBodyIndex, 0, bodyMaterials.Length - 1);
        foreach (var r in bodyRenderers)
            if (r != null) r.material = bodyMaterials[currentBodyIndex];
    }

    public void NextWheelStyle()
    {
        if (wheelMaterials.Length == 0 || wheelRenderers.Length == 0) return;
        currentWheelIndex = (currentWheelIndex + 1) % wheelMaterials.Length;
        ApplyWheelMaterial();
        Save();
    }

    void ApplyWheelMaterial()
    {
        if (wheelMaterials.Length == 0 || wheelRenderers.Length == 0) return;
        currentWheelIndex = Mathf.Clamp(currentWheelIndex, 0, wheelMaterials.Length - 1);
        foreach (var r in wheelRenderers)
            if (r != null) r.material = wheelMaterials[currentWheelIndex];
    }

    public void ToggleEngine()
    {
        engineOn = !engineOn;
        if (engineOn)
        {
            if (engineSource != null && engineStartClip != null)
                engineSource.PlayOneShot(engineStartClip);
        }
        else
        {
            if (engineSource != null) engineSource.Stop();
        }
    }

    public void PlayVoiceover()
    {
        if (voiceoverSource != null && !voiceoverSource.isPlaying)
            voiceoverSource.Play();
    }

    public void OpenAllDoors()
    {
        if (doorParts == null) return;
        foreach (var d in doorParts)
            if (d != null) d.OnTap();
    }

    public void ResetVehicle()
    {
        currentBodyIndex = 0;
        currentWheelIndex = 0;
        ApplyBodyMaterial();
        ApplyWheelMaterial();
        if (engineOn) ToggleEngine();
        transform.localScale = Vector3.one;
        Save();
    }

    void Start()
    {
        currentBodyIndex = PlayerPrefs.GetInt("BodyIdx_" + name, 0);
        currentWheelIndex = PlayerPrefs.GetInt("WheelIdx_" + name, 0);
        ApplyBodyMaterial();
        ApplyWheelMaterial();
    }

    void Save()
    {
        PlayerPrefs.SetInt("BodyIdx_" + name, currentBodyIndex);
        PlayerPrefs.SetInt("WheelIdx_" + name, currentWheelIndex);
    }
}