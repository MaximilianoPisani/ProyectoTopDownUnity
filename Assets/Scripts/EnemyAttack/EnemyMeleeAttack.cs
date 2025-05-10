using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : AttackMelee
{
    private Transform _target;

      public AttackData AttackData => _attackData;

   
    public float AttackRange => _attackData.AttackRange;

    private float attackTimer = 0f;

    private void Update()
    {
        if (_target != null)
        {
            float distance = Vector2.Distance(transform.position, _target.position);

            if (distance <= _attackData.AttackRange)
            {
                if (attackTimer <= 0f)
                {
                    _animatorHandler.SetBool("isAttacking", true);
                    attackTimer = _attackData.Cooldown;  
                }
            }
            else
            {
                _animatorHandler.SetBool("isAttacking", false);
            }
        }

       
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            SetTarget(other.transform);

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {

                ApplyDamage();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            SetTarget(null);
            _animatorHandler.SetBool("isAttacking", false);
        }
    }


    public void ApplyDamage()
    {
        if (_target != null)
        {
            PlayerHealth playerHealth = _target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)_attackData.Damage);
            }
        }
    }
}