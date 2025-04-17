using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AttackSystem
{
    public float attackRange = 1.5f;
    public LayerMask targetLayer;

    private CharacterStats stats;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public override void Attack()
    {
        int finalDamage = stats != null ? stats.attackPower : 10;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayer);

        foreach (var hit in hits)
        {
            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(finalDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}