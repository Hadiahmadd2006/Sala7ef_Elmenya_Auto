using UnityEngine;

public class VehicleRegistry : MonoBehaviour
{
    public static VehicleRegistry Instance { get; private set; }
    public VehicleRoot CurrentVehicle { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Register(VehicleRoot vehicle)
    {
        CurrentVehicle = vehicle;
    }

    public void Unregister(VehicleRoot vehicle)
    {
        if (CurrentVehicle == vehicle)
            CurrentVehicle = null;
    }
}