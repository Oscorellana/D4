using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Aiming Settings")]
    public bool aiming; // true when RMB is held

    private CharacterController controller;
    private Vector3 velocity;
    private float rotationVelocity;
    private bool isGrounded;
    private bool isSprinting;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleInput();
        HandleGravity();
        HandleJump();
    }

    void HandleInput()
    {
        if (Keyboard.current == null) return;

        // --- Movement input ---
        Vector2 moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;

        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // Sprint + Aim
        isSprinting = Keyboard.current.leftShiftKey.isPressed;
        aiming = Mouse.current != null && Mouse.current.rightButton.isPressed;

        float targetAngle = transform.eulerAngles.y;

        // --- Rotation ---
        if (aiming)
        {
            // Freeze rotation: face camera forward instantly
            targetAngle = cameraTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            rotationVelocity = 0f; // stop smoothing
        }
        else if (direction.magnitude >= 0.1f)
        {
            // Smoothly rotate toward movement direction relative to camera
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else
        {
            rotationVelocity = 0f; // idle, stop rotation
        }

        // --- Movement ---
        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDir;

            if (aiming)
            {
                // Movement relative to camera forward, but don't add forward bias
                Vector3 cameraForward = cameraTransform.forward;
                cameraForward.y = 0f;
                cameraForward.Normalize();

                Vector3 cameraRight = cameraTransform.right;
                cameraRight.y = 0f;
                cameraRight.Normalize();

                // Combine input with camera axes
                moveDir = cameraForward * direction.z + cameraRight * direction.x;
            }
            else
            {
                // Normal movement with rotation smoothing
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }

            float speed = isSprinting ? sprintSpeed : walkSpeed;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

    }

    void HandleGravity()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // keep grounded

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
