using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Body colour dropdown")]
    [SerializeField] private TMP_Dropdown bodyDropdown;
    [SerializeField] private string[] bodyHexCodes;

    [Header("Wheel colour dropdown")]
    [SerializeField] private TMP_Dropdown wheelDropdown;
    [SerializeField] private string[] wheelHexCodes;

    void Start()
    {
        if (bodyDropdown != null)
            bodyDropdown.onValueChanged.AddListener(OnBodyDropdownChanged);
        if (wheelDropdown != null)
            wheelDropdown.onValueChanged.AddListener(OnWheelDropdownChanged);
    }

    VehicleRoot Car()
    {
        if (VehicleRegistry.Instance == null) return null;
        return VehicleRegistry.Instance.CurrentVehicle;
    }

    void OnBodyDropdownChanged(int index)
    {
        VehicleRoot car = Car();
        if (car == null || car.colorManager == null) return;

        if (index == 0)
        {
            car.colorManager.ResetBodyToDefault();
            return;
        }

        if (bodyHexCodes == null || index >= bodyHexCodes.Length) return;
        car.colorManager.SetBodyColorHex(bodyHexCodes[index]);
    }

    void OnWheelDropdownChanged(int index)
    {
        VehicleRoot car = Car();
        if (car == null || car.colorManager == null) return;

        if (index == 0)
        {
            car.colorManager.ResetWheelToDefault();
            return;
        }

        if (wheelHexCodes == null || index >= wheelHexCodes.Length) return;
        car.colorManager.SetWheelColorHex(wheelHexCodes[index]);
    }

    public void OnEngineButton()
    {
        VehicleRoot car = Car();
        if (car != null && car.audioController != null)
            car.audioController.ToggleEngine();
    }

    public void OnVoiceoverButton()
    {
        VehicleRoot car = Car();
        if (car != null && car.audioController != null)
            car.audioController.PlayVoiceover();
    }

    public void OnTurntableButton()
    {
        VehicleRoot car = Car();
        if (car != null && car.turntable != null)
            car.turntable.ToggleSpin();
    }

    public void OnResetButton()
    {
        VehicleRoot car = Car();
        if (car == null) return;

        if (car.colorManager != null) car.colorManager.ResetColors();
        if (car.audioController != null) car.audioController.StopEngine();
        if (car.turntable != null) car.turntable.StopSpin();

        if (bodyDropdown != null)  bodyDropdown.SetValueWithoutNotify(0);
        if (wheelDropdown != null) wheelDropdown.SetValueWithoutNotify(0);
    }
}