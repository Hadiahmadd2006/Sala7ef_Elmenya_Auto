using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class MarkerSpawner : MonoBehaviour
{
    [System.Serializable] public struct ImageVehicle {
    public string imageName;
    public GameObject vehiclePrefab;
    }
    [SerializeField] private ImageVehicle[] mappings;
    private readonly Dictionary<string, GameObject> spawned = new();
    private ARTrackedImageManager manager;
    void Awake() => manager = GetComponent<ARTrackedImageManager>();
    void OnEnable() => manager.trackablesChanged.AddListener(OnChanged);
    void OnDisable() => manager.trackablesChanged.RemoveListener(OnChanged);
    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var img in args.added) Handle(img);
        foreach (var img in args.updated) Handle(img);
    }
    void Handle(ARTrackedImage img)
    {
        string name = img.referenceImage.name;
        if (!spawned.TryGetValue(name, out var go))
        {
            var prefab = System.Array.Find(mappings, m => m.imageName == name).vehiclePrefab;
            if (prefab == null) return;
            go = Instantiate(prefab, img.transform);
            spawned[name] = go;
        }
        bool isTracking = img.trackingState == TrackingState.Tracking;
        go.SetActive(isTracking);
    }
}