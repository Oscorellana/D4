using UnityEngine;

public class BillboardHealthBar : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset = Vector3.up * 4f;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;
        transform.position = transform.parent != null ? transform.parent.position + offset : transform.position;
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
