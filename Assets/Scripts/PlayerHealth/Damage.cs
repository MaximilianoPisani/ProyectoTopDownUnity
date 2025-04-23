using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageInterval = 1f;

    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                
                player.TakeDamage(damageAmount);

                
                damageCoroutine = StartCoroutine(DamageOverTime(player));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamageOverTime(PlayerHealth player)
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);

            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }
}