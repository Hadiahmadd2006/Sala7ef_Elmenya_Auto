using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    [SerializeField] private GameObject vehiclePrefab;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private GameObject configPanel;
    [SerializeField] private GestureManager gestureManager;

    private ARRaycastManager raycastManager;
    private GameObject placedVehicle;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        if (configPanel != null) configPanel.SetActive(false);

        if (gestureManager == null)
            gestureManager = FindFirstObjectByType<GestureManager>();
    }

    void Update()
    {
        if (Touchscreen.current == null) return;

        var touch = Touchscreen.current.primaryTouch;
        if (!touch.press.wasPressedThisFrame) return;

        int fingerId = (int)touch.touchId.ReadValue();
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId))
            return;

        Vector2 screenPos = touch.position.ReadValue();
        if (raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (placedVehicle == null)
            {
                placedVehicle = Instantiate(vehiclePrefab, hitPose.position, hitPose.rotation);

                foreach (var plane in planeManager.trackables)
                    plane.gameObject.SetActive(false);
                planeManager.enabled = false;

                if (configPanel != null) configPanel.SetActive(true);

                if (gestureManager != null)
                    gestureManager.Target = placedVehicle.transform;
            }
            else
            {
                placedVehicle.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            }
        }
    }

    public void SetVehiclePrefab(GameObject prefab) => vehiclePrefab = prefab;
    public GameObject GetPlacedVehicle() => placedVehicle;
}