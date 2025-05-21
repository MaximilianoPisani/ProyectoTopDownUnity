using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;
    private HealthSystem _healthSystem;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetHealthSystem(HealthSystem healthSystem) // Assigns a HealthSystem instance and sets up event subscriptions
    {
        if (_healthSystem != null)
        {
            _healthSystem.UnsubscribeFromHealthChanged(UpdateHealthBar);
        }

        _healthSystem = healthSystem;

        if (_healthSystem != null)
        {
            _healthSystem.SubscribeToHealthChanged(UpdateHealthBar);
            UpdateHealthBar(_healthSystem.CurrentHealth, _healthSystem.MaxHealth);
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth) // Updates the slider UI based on current and max health values
    {
        _slider.maxValue = maxHealth;
        _slider.value = currentHealth;
    }

    private void OnDestroy() // Called when the object is being destroyed
    {
        if (_healthSystem != null)
            _healthSystem.UnsubscribeFromHealthChanged(UpdateHealthBar);
    }
}