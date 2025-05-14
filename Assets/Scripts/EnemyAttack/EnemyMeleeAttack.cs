using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : AttackMelee
{
    private Transform _target;
    public float AttackRange => _attackData.AttackRange;
    public AttackData AttackData => _attackData;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void UpdateAttack()
    {
        if (_target == null) return;

        float distance = Vector2.Distance(transform.position, _target.position);

        if (distance <= AttackRange)
        {
            base.TryAttack();
        }
    }
    public void ApplyDamage()
    {
        if (_target == null) return;

        float distance = Vector2.Distance(transform.position, _target.position);

        if (distance <= AttackRange)
        {
            PlayerHealth playerHealth = _target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)_attackData.Damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetTarget(other.transform);
            GetComponent<EnemyMovement>().SetTarget(other.transform);
            GetComponent<EnemyMovement>().ChangeState(new EnemyAttackState());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            SetTarget(null);
    }
}