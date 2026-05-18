using UnityEngine;

public class CarColorManager : MonoBehaviour
{
    [Header("Body")]
    [SerializeField] private Renderer[] bodyRenderers;

    [Header("Wheels")]
    [SerializeField] private Renderer[] wheelRenderers;

    private Color bodyDefault;
    private Color wheelDefault;
    private bool defaultsSaved;

    void Awake()
    {
        SaveDefaults();
    }

    void SaveDefaults()
    {
        if (defaultsSaved) return;
        if (bodyRenderers != null && bodyRenderers.Length > 0 && bodyRenderers[0] != null)
            bodyDefault = ReadColor(bodyRenderers[0]);
        if (wheelRenderers != null && wheelRenderers.Length > 0 && wheelRenderers[0] != null)
            wheelDefault = ReadColor(wheelRenderers[0]);
        defaultsSaved = true;
    }

    public void SetBodyColorHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(Normalize(hex), out Color c))
            ApplyColor(bodyRenderers, c);
    }

    public void SetWheelColorHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(Normalize(hex), out Color c))
            ApplyColor(wheelRenderers, c);
    }

    public void ResetColors()
    {
        ApplyColor(bodyRenderers, bodyDefault);
        ApplyColor(wheelRenderers, wheelDefault);
    }

    void ApplyColor(Renderer[] renderers, Color c)
    {
        if (renderers == null) return;
        foreach (var r in renderers)
        {
            if (r == null) continue;
            Material m = r.material;
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
            if (m.HasProperty("_Color"))     m.SetColor("_Color", c);
        }
    }

    Color ReadColor(Renderer r)
    {
        Material m = r.material;
        if (m.HasProperty("_BaseColor")) return m.GetColor("_BaseColor");
        if (m.HasProperty("_Color"))     return m.GetColor("_Color");
        return Color.white;
    }

    string Normalize(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return "#000000";
        return hex.StartsWith("#") ? hex : "#" + hex;
    }
}