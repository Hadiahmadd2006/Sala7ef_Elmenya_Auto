using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class VehicleSwitcherAdvanced : MonoBehaviour
{
    [SerializeField] private ObjectSpawner spawner;
    [SerializeField] private Behaviour spawnTrigger;   
    [SerializeField] private Behaviour spawnOnce;      

    public void NextVehicle()
    {
        if (spawner == null) { Debug.Log("spawner NULL"); return; }

        int count = spawner.objectPrefabs.Count;
        if (count == 0) return;

        // Find currently placed car
        VehicleRoot currentCar = FindFirstObjectByType<VehicleRoot>();

        // Calculate next index
        int current = spawner.spawnOptionIndex;
        if (current < 0) current = 0;
        int next = (current + 1) % count;
        spawner.SetSpawnObjectIndex(next);

        if (currentCar != null)
        {
            // Save position/rotation of old car
            Vector3 pos = currentCar.transform.position;
            Quaternion rot = currentCar.transform.rotation;

            // Destroy old car
            Destroy(currentCar.gameObject);

            // Spawn new one at same spot
            GameObject newCar = Instantiate(
                spawner.objectPrefabs[next],
                pos,
                rot
            );
            Debug.Log("Switched to car " + next + " at same position");
        }
        else
        {
            // No car placed yet — just change index for next placement
            // Re-enable spawner in case SpawnOnce disabled it
            if (spawnTrigger != null) spawnTrigger.enabled = true;
            if (spawner != null) spawner.enabled = true;
            Debug.Log("No car placed — next placement will be car " + next);
        }
    }
}