using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;

public class GestureManager : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 0.2f;
    [SerializeField] private float pinchSpeed = 0.005f;
    [SerializeField] private Vector2 scaleClamp = new Vector2(0.3f, 2f);

    public Transform Target { get; set; }

    public bool IsUserDragging { get; private set; }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (Target == null)
            return;

        var touches = ETouch.Touch.activeTouches;
        if (touches.Count == 1)
        {
            HandleSingle(touches[0]);
        }
        else if (touches.Count == 2)
        {
            HandleDouble(touches[0], touches[1]);
        }
        else
        {
            IsUserDragging = false;
        }
    }

    void HandleSingle(ETouch.Touch t)
    {
        if (IsOverUI(t.finger.index))
            return;

        var phase = t.phase;
        if (phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            // Tap = raycast to vehicle parts (door, wheel, etc.)
            var cam = Camera.main;
            if (cam == null)
                return;

            Ray ray = cam.ScreenPointToRay(t.screenPosition);
            if (Physics.Raycast(ray, out var hit, 50f))
            {
                var part = hit.collider.GetComponentInParent<IInteractablePart>();
                part?.OnTap();
            }
        }
        else if (phase == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            // Drag = Y rotation
            IsUserDragging = true;
            float dx = t.delta.x;
            Target.Rotate(0f, -dx * rotateSpeed, 0f, Space.World);
        }
        else if (phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            IsUserDragging = false;
        }
    }

    void HandleDouble(ETouch.Touch a, ETouch.Touch b)
    {
        Vector2 prevA = a.screenPosition - a.delta;
        Vector2 prevB = b.screenPosition - b.delta;

        // Pinch: scale by distance change
        float prevDist = Vector2.Distance(prevA, prevB);
        float curDist = Vector2.Distance(a.screenPosition, b.screenPosition);
        float deltaDist = curDist - prevDist;
        float newScale = Mathf.Clamp(Target.localScale.x + deltaDist * pinchSpeed, scaleClamp.x, scaleClamp.y);
        Target.localScale = Vector3.one * newScale;

        // Two-finger rotation: angle delta between touches
        float prevAngle = Mathf.Atan2(prevB.y - prevA.y, prevB.x - prevA.x) * Mathf.Rad2Deg;
        float curAngle = Mathf.Atan2(b.screenPosition.y - a.screenPosition.y, b.screenPosition.x - a.screenPosition.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(prevAngle, curAngle);
        Target.Rotate(0f, -deltaAngle, 0f, Space.World);
    }

    static bool IsOverUI(int fingerId)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);
    }
}

public interface IInteractablePart
{
    void OnTap();
}