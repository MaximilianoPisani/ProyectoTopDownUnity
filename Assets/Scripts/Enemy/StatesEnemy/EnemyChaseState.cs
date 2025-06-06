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
        if (_enemy == null)
        {
            Debug.LogError("EnemyController is null in EnemyChaseState");
            return;
        }

        _enemy.agent.isStopped = false; 

        _enemy.SetMoving(true);
        _enemy.animHandler.SafeSetBool("Moving", true);
        _enemy.animHandler.SafeSetBool("isAttackingMelee", false);
        _enemy.animHandler.SafeSetBool("isAttackingRange", false);
    }
    public void UpdateState() // Called every frame while the enemy is in the chase state
    {
        if (_enemy == null)
        {
            Debug.LogError("EnemyController is null in EnemyChaseState.UpdateState");
            return;
        }

        if (_enemy.currentTarget != null)
        {
            Transform target = _enemy.currentTarget;
            var attackStrategy = _enemy.GetAttackStrategy();

            if (attackStrategy == null)
            {
                Debug.LogError("Attack strategy is null in EnemyChaseState.UpdateState");
                return;
            }

            float distance = Vector2.Distance(_enemy.transform.position, target.position);
            float attackRange = attackStrategy.GetAttackRange();

            if (distance <= attackRange)
            {
                _enemy.agent.isStopped = true;
                _enemy.agent.velocity = Vector3.zero;
                _enemy.GetComponent<EnemyStateMachine>().ChangeState(new EnemyAttackState());
                return;
            }

            if (_enemy.shouldChasePlayer)
            {
                _enemy.MoveTo(target.position);
                UpdateDirectionAnimation();
            }
            else
            {
                _enemy.agent.isStopped = true;
                _enemy.agent.velocity = Vector3.zero;
            }
        }
    }

    public void ExitState()  // Called when the enemy exits the chase state
    {
        SaveLastDirection();
        _enemy.animHandler.SafeSetBool("Moving", false);
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