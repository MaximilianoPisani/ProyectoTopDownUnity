using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 6;
    protected int health;

    public int CurrentHealth => health;
    public int MaxHealth => maxHealth;

    protected virtual void Start()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}