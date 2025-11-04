using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit a TargetDummy
        TargetDummy dummy = collision.collider.GetComponent<TargetDummy>();
        if (dummy != null)
        {
            dummy.TakeDamage(damage);
        }

        // Destroy bullet on any collision
        Destroy(gameObject);
    }
}
