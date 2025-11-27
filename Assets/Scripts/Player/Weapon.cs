using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    [Tooltip("Default base fire rate (seconds between shots)")]
    public float baseFireRate = 0.25f;

    private float nextFireTime = 0f;
    private PlayerUpgrade playerUpgrade;

    void Start()
    {
        playerUpgrade = GetComponent<PlayerUpgrade>(); // assume PlayerUpgrade sits on the same Player GameObject
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // effective fire rate uses player upgrade if available
        float effectiveFireRate = baseFireRate;
        if (playerUpgrade != null)
            effectiveFireRate = playerUpgrade.shotCooldown;

        if (Mouse.current.leftButton.isPressed)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + effectiveFireRate;
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;
        Destroy(bullet, 6f);
    }
}








// before Thanksgiving Overhaul

/*using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.25f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = firePoint.forward * bulletSpeed;
        Destroy(bullet, 5f);
    }
}*/
