using UnityEngine;

public class BillboardHealthBar : MonoBehaviour
{
    public Transform cameraTransform; // usually the main camera

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // Rotate to face the camera
            transform.LookAt(transform.position + cameraTransform.forward);
        }
    }
}
