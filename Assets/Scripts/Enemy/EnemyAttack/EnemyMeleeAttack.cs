using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : AttackMelee
{
    private Transform _target;
    public float AttackRange => attackData.AttackRange;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void UpdateAttack() // Called every frame to check if the target is within attack range and trigger an attack.
    {
        if (_target == null) return;

        float distance = Vector2.Distance(transform.position, _target.position);
        if (distance <= AttackRange)
        {
            TryAttack();
        }
    }

   
    public override void ApplyDamage() // Called by an animation event at specific frames where the attack should apply damage.
    {
        if (_target == null) return;

        float distance = Vector2.Distance(transform.position, _target.position);
        if (distance <= AttackRange)
        {
            PlayerHealth playerHealth = _target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)attackData.Damage);
            }
        }
    }

    public void EndAttackAnim() // Called by the last frame of the attack animation to mark the attack as finished.
    {
        EndAttack();
    }

    private void OnTriggerEnter2D(Collider2D other) // Triggered when the enemy detects the player entering its attack zone.
    {
        if (other.CompareTag("Player"))
        {
            SetTarget(other.transform);
            GetComponent<EnemyController>().SetTarget(other.transform);
            GetComponent<EnemyController>().ChangeState(new EnemyAttackMeleeState());
        }
    }

    private void OnTriggerExit2D(Collider2D other) // Triggered when the player leaves the enemy's detection range.
    {
        if (other.CompareTag("Player"))
            SetTarget(null);
    }
}