using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;
    public LayerMask targetLayer;

    private float timer;

    private void OnEnable()
    {
        timer = 0f; 
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            PoolManager.Instance.ReturnToPool(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}