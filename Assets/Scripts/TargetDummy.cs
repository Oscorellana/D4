using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    [Header("Health Settings")]
    public int health = 1; // number of hits to destroy

    // Called by bullet on collision
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // despawn dummy
        }
    }
}
