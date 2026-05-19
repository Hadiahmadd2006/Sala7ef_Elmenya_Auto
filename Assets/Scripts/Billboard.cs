using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        transform.LookAt(cam.transform);
        transform.Rotate(0f, 180f, 0f);
    }
}