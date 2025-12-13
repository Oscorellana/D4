using UnityEngine;
using System;

public class TargetDummy : MonoBehaviour
{
    public int maxHealth = 3;
    int hp;

    public Action OnDeath;

    void Start() => hp = maxHealth;

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) Die();
    }

    void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
