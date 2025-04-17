using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int MaxHealth = 100;
    protected int _currentHealth;
    protected bool _canTakeDamage = true;
    public float DamageCooldown = 1f;

    protected virtual void Start()
    {
        _currentHealth = MaxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (!_canTakeDamage) return;

        _currentHealth -= damage;
        _canTakeDamage = false;
        StartCoroutine(DamageCooldownRoutine()); 

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageCooldownRoutine()
    {
        yield return new WaitForSeconds(DamageCooldown);
        _canTakeDamage = true;
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false); 
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }
}