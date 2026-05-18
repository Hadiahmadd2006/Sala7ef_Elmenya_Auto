using UnityEngine;
using UnityEngine.UI;
using TMPro;   // top of file

public class ColorDropdown : MonoBehaviour
{
    [System.Serializable]
    public struct CarEntry
    {
        public string carName;            // just a label for you
        public Renderer targetRenderer;   // the black-material mesh on THIS car
        public Material defaultMaterial;  // this car's original material
    }

    [Header("Cars - add each car with its renderer and default material")]
    [SerializeField] private CarEntry[] cars;

    [Header("Which car is active (0 = first car, 1 = second car)")]
    [SerializeField] private int activeCarIndex = 0;

    [Header("UI")]
    [SerializeField] private TMP_Dropdown colorDropdown;   // change the type
    
    [System.Serializable]
    public struct ColorOption
    {
        public string label;   // dropdown label (e.g. "Red")
        public string hex;     // hex code (e.g. "FF0000")
    }

    [Header("Colors - order must match dropdown options")]
    [SerializeField] private ColorOption[] colorOptions;

    void Start()
    {
        if (colorDropdown != null)
        {
            colorDropdown.onValueChanged.AddListener(OnColorChanged);
            OnColorChanged(colorDropdown.value);
        }
    }

    // Call this when the player selects a car (e.g. from a car-select button or dropdown)
    public void SetActiveCar(int carIndex)
    {
        if (cars == null || carIndex < 0 || carIndex >= cars.Length) return;
        activeCarIndex = carIndex;

        // Re-apply current dropdown color to the newly selected car
        if (colorDropdown != null)
            OnColorChanged(colorDropdown.value);
    }

    // Called when the color dropdown changes
    public void OnColorChanged(int colorIndex)
    {
        CarEntry car = GetActiveCar();
        if (car.targetRenderer == null) return;
        if (colorOptions == null || colorIndex < 0 || colorIndex >= colorOptions.Length) return;

        if (ColorUtility.TryParseHtmlString(NormalizeHex(colorOptions[colorIndex].hex), out Color color))
        {
            ApplyColor(car.targetRenderer, color);
        }
        else
        {
            Debug.LogWarning("Bad hex code: " + colorOptions[colorIndex].hex);
        }
    }

    // Resets the active car back to its default material
    public void ResetActiveCarMaterial()
    {
        CarEntry car = GetActiveCar();
        if (car.targetRenderer != null && car.defaultMaterial != null)
            car.targetRenderer.material = car.defaultMaterial;
    }

    CarEntry GetActiveCar()
    {
        if (cars == null || activeCarIndex < 0 || activeCarIndex >= cars.Length)
            return default;
        return cars[activeCarIndex];
    }

    void ApplyColor(Renderer rend, Color color)
    {
        Material mat = rend.material;   // instance - only this car affected
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color"))     mat.SetColor("_Color", color);
    }

    string NormalizeHex(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return "#000000";
        return hex.StartsWith("#") ? hex : "#" + hex;
    }
}