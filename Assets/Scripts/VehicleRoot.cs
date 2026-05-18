using UnityEngine;
public class VehicleRoot : MonoBehaviour
{
    public CarColorManager colorManager;
    public CarAudio audioController;
    public Turntable turntable;
    void Start()
    {
        if (VehicleRegistry.Instance != null)
            VehicleRegistry.Instance.Register(this);
    }
    void OnDestroy()
    {
        // Clean up if the car is removed.
        if (VehicleRegistry.Instance != null)
            VehicleRegistry.Instance.Unregister(this);
    }
}