using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public static class SessionData
{
    public static int SelectedVehicleIndex = 0;
}

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    [SerializeField] private GameObject vehiclePrefab; 
    [SerializeField] private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;
    private GameObject placedVehicle;
    private static List<ARRaycastHit> hits = new();
    void Awake() => raycastManager = GetComponent<ARRaycastManager>();
    void Update()
    {
        if (Touchscreen.current == null) return;
        var touch = Touchscreen.current.primaryTouch;
        if (touch.press.wasPressedThisFrame == false) return;
        // Don't place if the tap was on a UI element
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(
        touch.touchId.ReadValue())) return;
        Vector2 screenPos = touch.position.ReadValue();
        if (raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            if (placedVehicle == null)
                placedVehicle = Instantiate(vehiclePrefab, hitPose.position, hitPose.rotation);
            else
                placedVehicle.transform.SetPositionAndRotation(hitPose.position,
                hitPose.rotation);
            foreach (var p in planeManager.trackables) p.gameObject.SetActive(false);
            planeManager.enabled = false;
        }
    }
    public void SetVehiclePrefab(GameObject prefab) => vehiclePrefab = prefab;
}