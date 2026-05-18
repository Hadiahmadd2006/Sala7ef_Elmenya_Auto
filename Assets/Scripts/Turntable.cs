using UnityEngine;
public class Turntable : MonoBehaviour
{
    [Header("Rotation speed in degrees per second")]
    [SerializeField] private float speed = 20f;
    private bool spinning;
    public void ToggleSpin()
    {
        spinning = !spinning;
    }
    public void StopSpin()
    {
        spinning = false;
    }
    void Update()
    {
        if (!spinning) return; 
        transform.Rotate(0f, speed * Time.deltaTime, 0f, Space.World);
    }
}