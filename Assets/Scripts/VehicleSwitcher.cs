using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class VehicleSwitcher : MonoBehaviour
{
    [SerializeField] private ObjectSpawner spawner;

    public void NextVehicle()
    {
        if (spawner == null) return;

        int count = spawner.objectPrefabs.Count;
        if (count == 0) return;

        int current = spawner.spawnOptionIndex;
        if (current < 0) current = 0;          

        int next = (current + 1) % count;    
        spawner.SetSpawnObjectIndex(next);
    }
}