using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;              // Automatically found by tag
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Movement Settings")]
    public float detectionRange = 25f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float stopDistance = 5f;

    [Header("Shooting Settings")]
    public float shootInterval = 1.5f;
    private float shootTimer = 0f;

    void Start()
    {
        // Automatically find the player in the scene using the "Player" tag
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
                player = found.transform;
            else
                Debug.LogWarning($"{name} could not find a GameObject tagged 'Player'!");
        }
    }

    void Update()
    {
        if (player == null) return;

        // Draw a red line to the player in Scene view for debugging
        Debug.DrawLine(transform.position + Vector3.up, player.position + Vector3.up, Color.red);

        float distance = Vector3.Distance(transform.position, player.position);

        // Detect player within range
        if (distance <= detectionRange)
        {
            Debug.Log($"{name} sees player at {distance:F1} meters");

            FacePlayer();

            if (distance > stopDistance)
            {
                MoveTowardPlayer();
            }
            else
            {
                TryShoot();
            }
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // keep horizontal rotation only

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void MoveTowardPlayer()
    {
        // Move straight toward player until within stopDistance
        Vector3 targetPosition = player.position - transform.forward * stopDistance;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void TryShoot()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = shootInterval;

            if (bulletPrefab != null && firePoint != null)
            {
                Debug.Log($"{name} fired a bullet!");

                // Add slight random aim spread
                Vector3 shootDir = (player.position + Vector3.up * 1.5f - firePoint.position).normalized;
                shootDir = Quaternion.Euler(
                    Random.Range(-3f, 3f),
                    Random.Range(-3f, 3f),
                    0f
                ) * shootDir;

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDir));

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(shootDir * 25f, ForceMode.Impulse);
                }

                Destroy(bullet, 3f);
            }
            else
            {
                Debug.LogWarning($"{name} cannot shoot — missing firePoint or bulletPrefab reference!");
            }
        }
    }
}
