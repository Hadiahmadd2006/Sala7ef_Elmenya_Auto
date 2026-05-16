using UnityEngine;

public class WheelSpinner : MonoBehaviour
{
    public float speedDegPerSec = 360f;
    public bool Spinning { get; set; }

    void Update()
    {
        if (Spinning)
        {
            transform.Rotate(speedDegPerSec * Time.deltaTime, 0f, 0f, Space.Self);
        }
    }
}