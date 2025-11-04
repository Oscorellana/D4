using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        TargetDummy dummy = collision.collider.GetComponent<TargetDummy>();
        if (dummy != null)
        {
            dummy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
