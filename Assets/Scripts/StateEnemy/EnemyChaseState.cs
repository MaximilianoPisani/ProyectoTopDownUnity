using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private EnemyMovement _enemy;

    public void EnterState(EnemyMovement enemy)
    {
        _enemy = enemy;
        _enemy.SetMoving(true);
    }

    public void UpdateState()
    {
        if (_enemy.CurrentTarget != null)
        {
            float distance = Vector2.Distance(_enemy.transform.position, _enemy.CurrentTarget.position);

            if (distance <= _enemy.GetComponent<EnemyMeleeAttack>().AttackRange)
            {
                _enemy.ChangeState(new EnemyAttackState());
                return;
            }

            _enemy.MoveTo(_enemy.CurrentTarget.position);
        }
    }

    public void ExitState()
    {
        _enemy.SetMoving(false);
    }
}