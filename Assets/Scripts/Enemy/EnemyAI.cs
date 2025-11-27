using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;              // auto found if null
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
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) player = found.transform;
            else Debug.LogWarning($"{name} could not find a GameObject tagged 'Player'!");
        }
    }

    void Update()
    {
        if (player == null) return;

        Debug.DrawLine(transform.position + Vector3.up, player.position + Vector3.up, Color.red);
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            FacePlayer();
            if (distance > stopDistance) MoveTowardPlayer();
            else TryShoot();
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void MoveTowardPlayer()
    {
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
                Vector3 shootDir = (player.position + Vector3.up * 1.5f - firePoint.position).normalized;
                shootDir = Quaternion.Euler(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f) * shootDir;
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDir));
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null) rb.linearVelocity = shootDir * 20f;
                Destroy(bullet, 3f);
            }
            else Debug.LogWarning($"{name} cannot shoot — missing firePoint or bulletPrefab reference!");
        }
    }
}
