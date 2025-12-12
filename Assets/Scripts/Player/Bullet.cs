using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start() => Destroy(gameObject, lifeTime);

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<TargetDummy>(out var dummy))
        {
            PlayerUpgrade pu = FindFirstObjectByType<PlayerUpgrade>();
            int dmg = pu != null ? pu.damage : 1;
            dummy.TakeDamage(dmg);
        }
        Destroy(gameObject);
    }
}
