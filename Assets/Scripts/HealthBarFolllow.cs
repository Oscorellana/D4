using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            // Optional: face the camera
            Camera cam = Camera.main;
            if (cam != null)
                transform.LookAt(transform.position + cam.transform.forward);
        }
    }
}
