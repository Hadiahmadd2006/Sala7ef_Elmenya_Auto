using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void LateUpdate()
    {
        if (cam == null)
            cam = Camera.main;
        if (cam == null)
            return;

        transform.LookAt(cam.transform);
        transform.Rotate(0f, 180f, 0f);
    }
}