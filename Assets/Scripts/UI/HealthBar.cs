using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void ChangeMaxHealth(float maxHealth)
    {
        _slider.maxValue = maxHealth;
    }

    public void ChangeCurrentHealth(float currentHealth)
    {
        _slider.value = currentHealth;
    }

    public void InitializeHealthBar(float initialHealth)
    {
        ChangeMaxHealth(initialHealth);
        ChangeCurrentHealth(initialHealth);
    }
}