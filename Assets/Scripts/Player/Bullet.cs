using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3f;

    private void Start()
    {
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<TargetDummy>(out var enemy))
        {
            PlayerUpgrade player = FindAnyObjectByType<PlayerUpgrade>();
            enemy.TakeDamage(player.damage);
        }

        Destroy(gameObject);
    }
}
