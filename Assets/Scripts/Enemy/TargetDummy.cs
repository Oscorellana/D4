using UnityEngine;
using System;

public class TargetDummy : MonoBehaviour
{
    public int maxHealth = 3;
    int hp;

    public EnemyHealthBar healthBar;

    public Action OnDeath;

    void Start()
    {
        hp = maxHealth;

        if (healthBar != null)
            healthBar.Init(maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (healthBar != null)
            healthBar.SetHealth(hp);

        if (hp <= 0)
            Die();
    }

    void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
