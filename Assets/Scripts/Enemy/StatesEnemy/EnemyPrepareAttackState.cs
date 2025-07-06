using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrepareAttackState : IEnemyState
{
    private EnemyController _enemy;
    private IAttackStrategy _attackStrategy;
    private float _prepareTimer;
    private float _prepareDuration;

    public EnemyPrepareAttackState(float prepareDuration)
    {
        _prepareDuration = prepareDuration;
    }

    public void EnterState(EnemyController enemy)
    {
        _enemy = enemy;
        _attackStrategy = _enemy.GetAttackStrategy();
        _prepareTimer = 0f;

        _enemy.PauseMovement();

        _enemy.animHandler.SafeSetBool("isAttackingMelee", false);
        _enemy.animHandler.SafeSetBool("isAttackingRange", false);
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

        _prepareTimer += Time.deltaTime;

        if (_prepareTimer >= _prepareDuration)
        {
            _enemy.GetComponent<EnemyStateMachine>().ChangeState(new EnemyAttackState());
        }
    }

    public void ExitState()
    {

    }
}