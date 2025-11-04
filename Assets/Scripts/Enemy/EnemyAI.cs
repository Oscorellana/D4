using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;            // Assign the player in inspector
    public GameObject bulletPrefab;     // Enemy bullet prefab
    public Transform firePoint;         // Where bullets spawn from (child object)

    [Header("Movement Settings")]
    public float detectionRange = 20f;  // How close player must be to chase
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float stopDistance = 5f;     // Distance to stop before player

    [Header("Shooting Settings")]
    public float shootInterval = 1.5f;
    private float shootTimer = 0f;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            FacePlayer();

            if (distance > stopDistance)
                MoveTowardPlayer();
            else
                TryShoot();
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // prevent tilting up/down

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void MoveTowardPlayer()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void TryShoot()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = shootInterval;

            if (bulletPrefab != null && firePoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddForce(firePoint.forward * 20f, ForceMode.Impulse);

                Destroy(bullet, 3f); // cleanup
            }
        }
    }
}
