using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 6;
    protected int health;

    public int CurrentHealth => health;
    public int MaxHealth => maxHealth;

    
    protected event Action<int, int> OnHealthChanged;

    protected virtual void Start()
    {
        health = maxHealth;
        NotifyHealthChanged();
    }

    public virtual void TakeDamage(int amount) // Method to apply damage to the entity.
    {
        health -= amount;
        if (health < 0) health = 0;

        NotifyHealthChanged();

        if (health <= 0)
        {
            Die();
        }
    }

   
    protected void NotifyHealthChanged() // Notifies all subscribers of the current and max health.
    {
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    protected virtual void Die()  // Method that handles what happens when the entity dies 
    {
        Destroy(gameObject);
    }

   
    public void SubscribeToHealthChanged(Action<int, int> observer) // Method to subscribe to the health change event.
    {
        OnHealthChanged += observer;
    }

    public void UnsubscribeFromHealthChanged(Action<int, int> observer) // Method to unsubscribe from the health change event.
    {
        OnHealthChanged -= observer;
    }
}
