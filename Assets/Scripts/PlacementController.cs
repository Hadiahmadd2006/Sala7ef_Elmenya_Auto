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
    [SerializeField] private GameObject configPanel; // optional UI panel to show after initial placement

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
            {
                // 1. Spawns the car
                placedVehicle = Instantiate(vehiclePrefab, hitPose.position, hitPose.rotation);

                // 2. ---> NEW LINE: Tells the GestureManager to control this specific car <---
                FindObjectOfType<GestureManager>().Target = placedVehicle.transform;

                // 3. Hides the planes
                foreach (var plane in planeManager.trackables)
                    plane.gameObject.SetActive(false);
                planeManager.enabled = false;

                if (configPanel != null) configPanel.SetActive(true);
            }
            else
            {
                placedVehicle.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            }
        }
    }

    public void SetVehiclePrefab(GameObject prefab) => vehiclePrefab = prefab;
}