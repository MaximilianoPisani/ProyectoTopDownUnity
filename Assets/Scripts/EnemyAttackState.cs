using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState //#TEST
{
    private EnemyController _enemy;
    private IAttackStrategy _attackStrategy;

    public void EnterState(EnemyController enemy)
    {
        _enemy = enemy;
        _attackStrategy = _enemy.GetAttackStrategy();

        _enemy.agent.isStopped = true;
        _enemy.agent.velocity = Vector3.zero;
        _enemy.agent.ResetPath();

        _enemy.animHandler.SafeSetBool("isAttackingMelee", _attackStrategy is MeleeAttackStrategy);
        Debug.Log("Is Ranged Attack: " + (_attackStrategy is RangedAttackStrategy));
        _enemy.animHandler.SafeSetBool("isAttackingRange", _attackStrategy is RangedAttackStrategy);
    }

    public void UpdateState()
    {
        if (_enemy.currentTarget == null)
        {
            _enemy.GetComponent<EnemyStateMachine>().ChangeState(new EnemyPatrolState());
            return;
        }

        float distance = Vector2.Distance(_enemy.transform.position, _enemy.currentTarget.position);

        if (distance > _attackStrategy.GetAttackRange())
        {
            _enemy.GetComponent<EnemyStateMachine>().ChangeState(new EnemyChaseState());
            return;
        }

        _attackStrategy.ExecuteAttack(_enemy);
    }

    public void ExitState()
    {
        _enemy.agent.isStopped = false;

        _enemy.animHandler.SafeSetBool("isAttackingMelee", false);
        _enemy.animHandler.SafeSetBool("isAttackingRange", false);
    }
}