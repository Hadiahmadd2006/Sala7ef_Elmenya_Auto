using UnityEngine;
public class VehicleRegistry : MonoBehaviour
{
    // The single shared instance other scripts use.
    public static VehicleRegistry Instance { get; private set; }
    // The car currently spawned in the scene (null if none yet).
    public VehicleRoot CurrentVehicle { get; private set; }
    void Awake()
    {
        // Standard singleton pattern.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Called by VehicleRoot when a car spawns.
    public void Register(VehicleRoot vehicle)
    {
        CurrentVehicle = vehicle;
    }
    // Called by VehicleRoot when a car is destroyed.
    public void Unregister(VehicleRoot vehicle)
    {
        if (CurrentVehicle == vehicle)
            CurrentVehicle = null;
    }
}