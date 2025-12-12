using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float baseFireRate = 0.25f;

    float nextFireTime = 0f;
    PlayerUpgrade playerUpgrade;

    void Start()
    {
        playerUpgrade = FindFirstObjectByType<PlayerUpgrade>();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        float effectiveRate = baseFireRate;
        if (playerUpgrade != null) effectiveRate = playerUpgrade.fireRate;

        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + effectiveRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = b.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;
        Destroy(b, 6f);
    }
}
