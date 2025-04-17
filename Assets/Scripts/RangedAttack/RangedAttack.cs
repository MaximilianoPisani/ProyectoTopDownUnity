using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : AttackSystem
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    private CharacterStats stats;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public override void Attack()
    {
        if (bulletPrefab == null || firePoint == null) return;

        
        GameObject bullet = PoolManager.Instance.GetFromPool(bulletPrefab, firePoint.position, firePoint.rotation);

       
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null && stats != null)
        {
            bulletScript.damage = stats.attackPower;
        }

       
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * bulletSpeed;
        }
    }
}