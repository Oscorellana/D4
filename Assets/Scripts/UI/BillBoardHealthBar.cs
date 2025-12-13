using UnityEngine;

public class BillboardHealthBar : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset = Vector3.up * 2f;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;
        if (transform.parent != null) transform.position = transform.parent.position + offset;
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
