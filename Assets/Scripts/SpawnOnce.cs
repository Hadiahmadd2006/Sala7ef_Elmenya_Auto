using UnityEngine;

public class SpawnOnce : MonoBehaviour
{
    [SerializeField] private Behaviour spawnTrigger;   
    [SerializeField] private Behaviour objectSpawner;  

    private bool used;

    public void OnSpawned()
    {
        if (used) return;
        used = true;

        if (spawnTrigger != null) spawnTrigger.enabled = false;
        if (objectSpawner != null) objectSpawner.enabled = false;
    }
}