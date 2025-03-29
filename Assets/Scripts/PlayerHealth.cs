using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int _currentHealth;
    private bool _canTakeDamage = true;
    public float damageCooldown = 1.5f;

    void Start()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            _currentHealth -= damage;
            _canTakeDamage = false;
            Invoke(nameof(ResetDamageCooldown), damageCooldown);
        }
    }

    private void ResetDamageCooldown()
    {
        _canTakeDamage = true;
    }
}
