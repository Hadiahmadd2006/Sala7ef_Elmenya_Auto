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
    [SerializeField] private WheelSpinner[] wheels;
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
        // Body: read parent, grab all child renderers (any renderer type)
        bodyRenderers = (bodyParent != null)
            ? bodyParent.GetComponentsInChildren<Renderer>(true)
            : new Renderer[0];

        // Wheels: read parent, grab all child renderers
        wheelRenderers = (wheelsParent != null)
            ? wheelsParent.GetComponentsInChildren<Renderer>(true)
            : new Renderer[0];

        // Auto-find WheelSpinners under wheels parent if array left empty
        if ((wheels == null || wheels.Length == 0) && wheelsParent != null)
            wheels = wheelsParent.GetComponentsInChildren<WheelSpinner>(true);

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

    // ---------- BODY COLOR ----------
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

    // ---------- WHEEL STYLE ----------
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

    // ---------- ENGINE (startup sound only, no loop) ----------
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
        if (wheels != null)
            foreach (var w in wheels)
                if (w != null) w.Spinning = engineOn;
    }

    // ---------- VOICEOVER ----------
    public void PlayVoiceover()
    {
        if (voiceoverSource != null && !voiceoverSource.isPlaying)
            voiceoverSource.Play();
    }

    // ---------- DOORS ----------
    public void OpenAllDoors()
    {
        if (doorParts == null) return;
        foreach (var d in doorParts)
            if (d != null) d.OnTap();
    }

    // ---------- RESET ----------
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

    // ---------- SAVE / LOAD ----------
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