using UnityEngine;

public class SpawnOnce : MonoBehaviour
{
    [SerializeField] private Behaviour spawnTrigger;   // drag AR Interactor Spawn Trigger
    [SerializeField] private Behaviour objectSpawner;  // drag Object Spawner

    private bool used;

    // Hook this to the "Object Spawn Triggered" event in Inspector
    public void OnSpawned()
    {
        if (used) return;
        used = true;

        if (spawnTrigger != null) spawnTrigger.enabled = false;
        if (objectSpawner != null) objectSpawner.enabled = false;
    }
}