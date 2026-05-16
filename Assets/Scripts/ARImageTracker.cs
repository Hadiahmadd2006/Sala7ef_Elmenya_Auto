using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[Serializable]
public class ImagePrefabEntry
{
    public string imageName;
    public GameObject prefab;
}

public class ARImageTracker : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private List<ImagePrefabEntry> imagePrefabs;

    private Dictionary<string, GameObject> _prefabLookup = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (var entry in imagePrefabs)
        {
            if (entry != null && entry.prefab != null
                && !_prefabLookup.ContainsKey(entry.imageName))
            {
                _prefabLookup[entry.imageName] = entry.prefab;
            }
        }
    }

    private void OnEnable()
    {
        if (imageManager != null)
            imageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    private void OnDisable()
    {
        if (imageManager != null)
            imageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)   HandleImageAdded(trackedImage);
        foreach (var trackedImage in eventArgs.updated) HandleImageUpdated(trackedImage);
        foreach (var pair in eventArgs.removed)         HandleImageRemoved(pair.Value);
    }

    private void HandleImageAdded(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        if (_prefabLookup.TryGetValue(imageName, out GameObject prefab))
        {
            GameObject spawnedContent = Instantiate(
                prefab,
                trackedImage.transform.position,
                trackedImage.transform.rotation,
                trackedImage.transform
            );
        }
    }

private void HandleImageUpdated(ARTrackedImage trackedImage)
{
    if (trackedImage.transform.childCount == 0) return;

    GameObject content = trackedImage.transform.GetChild(0).gameObject;

    switch (trackedImage.trackingState)
    {
        case TrackingState.Tracking:
            content.SetActive(true);
            break;
        case TrackingState.Limited:
            content.SetActive(true);
            break;
        case TrackingState.None:
            content.SetActive(false);
            break;
    }
}

    private void HandleImageRemoved(ARTrackedImage trackedImage)
    {
        if (trackedImage != null)
            Debug.Log("Image removed: " + trackedImage.referenceImage.name);
    }

    // Sets color on both URP (_BaseColor) and Built-in (_Color) shaders.
    private void SetColor(Renderer rend, Color color)
    {
        if (rend == null) return;
        Material mat = rend.material;
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
        if (mat.HasProperty("_Color"))     mat.SetColor("_Color", color);
    }
}