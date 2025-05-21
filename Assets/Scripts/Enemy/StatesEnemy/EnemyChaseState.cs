using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IEnemyState
{
    private EnemyController _enemy;

    public void EnterState(EnemyController enemy) // Called when the enemy enters the chase state
    {
        _enemy = enemy;
        _enemy.SetMoving(true);
        _enemy.animHandler.SetBool("Moving", true);
        _enemy.animHandler.SetBool("isAttacking", false);
    }

    public void UpdateState() // Called every frame while the enemy is in the chase state
    {
        if (_enemy.currentTarget != null)
        {
            float distance = Vector2.Distance(_enemy.transform.position, _enemy.currentTarget.position);

            if (distance <= _enemy.GetComponent<EnemyMeleeAttack>().AttackRange)
            {
                _enemy.agent.isStopped = true;
                _enemy.agent.velocity = Vector3.zero;
                _enemy.ChangeState(new EnemyAttackState());
                return;
            }

            _enemy.MoveTo(_enemy.currentTarget.position);
            UpdateDirectionAnimation();
        }

    }
    public void ExitState()  // Called when the enemy exits the chase state
    {
        SaveLastDirection();
        _enemy.animHandler.SetBool("Moving", false);
        _enemy.SetMoving(false);
    }

    private void SaveLastDirection() // Save the last moving direction for animations when exiting this state
    {
        Vector2 vel = _enemy.agent.velocity;
        if (vel.sqrMagnitude > 0.01f)
        {
            vel.Normalize();
            _enemy.animHandler.SetFloat("LastX", vel.x);
            _enemy.animHandler.SetFloat("LastY", vel.y);
        }
    }

    private void UpdateDirectionAnimation() // Update animation parameters to reflect movement direction
    {
        Vector2 vel = _enemy.agent.velocity;
        if (vel.sqrMagnitude > 0.01f)
        {
            vel.Normalize();
            _enemy.animHandler.SetFloat("X", vel.x);
            _enemy.animHandler.SetFloat("Y", vel.y);

        }
    }
}