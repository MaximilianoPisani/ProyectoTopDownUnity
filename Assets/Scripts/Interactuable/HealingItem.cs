using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [SerializeField] private int healAmount = 10;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        if (playerHealth != null && !playerHealth.IsDead() && playerHealth.CurrentHealth < playerHealth.MaxHealth)
        {
            playerHealth.Heal(healAmount);
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }

}