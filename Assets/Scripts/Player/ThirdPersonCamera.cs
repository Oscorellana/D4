using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -4f);

    [Header("Camera Movement")]
    public float rotationSpeed = 120f;
    public float mouseSensitivity = 1f;
    public float smoothTime = 0.1f;

    [Header("Vertical Look Limits")]
    public float minPitch = -30f;
    public float maxPitch = 60f;

    [Header("Zoom Settings")]
    public float minZoom = -2f;
    public float maxZoom = -8f;
    public float zoomSpeed = 2f;

    [Header("Collision Settings")]
    public LayerMask collisionLayers;
    public float cameraRadius = 0.3f;
    public float minDistance = 0.5f;
    public float collisionSmooth = 0.05f;

    private float yaw;
    private float pitch;
    private Vector3 currentVelocity;
    private Vector3 desiredCameraPos;
    private float currentZoom;

    void Start()
    {
        if (target == null) Debug.LogWarning("ThirdPersonCamera: No target assigned!");
        Vector3 euler = transform.eulerAngles;
        yaw = euler.y; pitch = euler.x;
        currentZoom = offset.z;
    }

    void LateUpdate()
    {
        if (target == null) return;
        HandleRotation();
        HandleZoom();
        HandlePosition();
    }

    void HandleRotation()
    {
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            yaw += mouseDelta.x * rotationSpeed * mouseSensitivity * Time.deltaTime;
            pitch -= mouseDelta.y * rotationSpeed * mouseSensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void HandleZoom()
    {
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                currentZoom += scroll * zoomSpeed * Time.deltaTime;
                currentZoom = Mathf.Clamp(currentZoom, maxZoom, minZoom);
            }
        }
        offset.z = Mathf.Lerp(offset.z, currentZoom, 10f * Time.deltaTime);
    }

    void HandlePosition()
    {
        Vector3 targetPos = target.position;
        desiredCameraPos = targetPos + transform.rotation * offset;

        if (Physics.SphereCast(targetPos, cameraRadius, (desiredCameraPos - targetPos).normalized,
            out RaycastHit hit, Mathf.Abs(offset.z), collisionLayers, QueryTriggerInteraction.Ignore))
        {
            float distance = Mathf.Clamp(hit.distance, minDistance, Mathf.Abs(offset.z));
            Vector3 collisionOffset = Vector3.forward * -distance;
            Vector3 adjustedPos = targetPos + transform.rotation * collisionOffset;
            transform.position = Vector3.Lerp(transform.position, adjustedPos, collisionSmooth / Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredCameraPos, ref currentVelocity, smoothTime);
        }
    }
}
