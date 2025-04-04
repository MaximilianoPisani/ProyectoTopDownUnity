using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            _canTakeDamage = false;
            Invoke(nameof(ResetDamageCooldown), damageCooldown);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void ResetDamageCooldown()
    {
        _canTakeDamage = true;
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}