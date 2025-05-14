using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAttackState : IEnemyState
{
    private EnemyMovement _enemy;
    private EnemyMeleeAttack _attack;

    public void EnterState(EnemyMovement enemy)
    {
        _enemy = enemy;
        _attack = enemy.GetComponent<EnemyMeleeAttack>();
        _attack.SetTarget(enemy.CurrentTarget);
        _enemy.SetMoving(false);
    }

    public void UpdateState()
    {
        if (_enemy.CurrentTarget == null)
        {
            _enemy.ChangeState(new EnemyPatrolState());
            return;
        }

        float distance = Vector2.Distance(_enemy.transform.position, _enemy.CurrentTarget.position);

        if (distance <= _attack.AttackRange)
        {
            _attack.UpdateAttack();
        }
        else
        {
            _enemy.ChangeState(new EnemyChaseState());
        }
    }

    public void ExitState()
    {
        _attack.SetTarget(null);
    }
}