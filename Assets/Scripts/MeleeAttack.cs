using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AttackSystem
{
    public int damage = 10;
    public float attackRange = 1.5f;
    public LayerMask targetLayer;

    public override void Attack()
    {
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayer);

        foreach (var hit in hits)
        {
            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}