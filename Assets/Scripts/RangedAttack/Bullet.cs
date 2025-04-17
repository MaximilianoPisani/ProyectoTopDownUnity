using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 2;
    public float lifetime = 3f;
    public LayerMask targetLayer;

    private float _timer;

    private void OnEnable()
    {
        _timer = 0f; 
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= lifetime)
        {
            ReturnToPool();
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

        ReturnToPool(); 
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false); 
        PoolManager.Instance.ReturnToPool(gameObject); 
    }
}