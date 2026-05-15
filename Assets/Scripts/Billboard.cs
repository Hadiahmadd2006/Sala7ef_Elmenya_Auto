using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Make the UI look at the camera
        transform.LookAt(Camera.main.transform);

        // Flip it 180 degrees so the text isn't mirrored
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
    }
}