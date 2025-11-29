using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;

    // Event fired when the enemy dies
    public System.Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Fire the event so SpawnManager knows this enemy died
        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}
