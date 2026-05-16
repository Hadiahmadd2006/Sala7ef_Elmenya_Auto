using UnityEngine;
using UnityEngine.UI;
public class Turntable : MonoBehaviour
{
    [SerializeField] private GestureManager gestures;
    [SerializeField] private Slider speedSlider; // 0..30 deg/sec
    // Indicates whether automatic turning is active.
    public bool IsActive { get; private set; }
    // Toggle the active state.
    public void Toggle() => IsActive = !IsActive;
    private void Update()
    {
        if (!IsActive) return;
        // Drag gesture overrides turntable (rubric requirement)
        if (gestures != null && gestures.IsUserDragging) return;
        float speed = speedSlider != null ? speedSlider.value : 10f;
        transform.Rotate(0f, speed * Time.deltaTime, 0f, Space.World);
    }
}