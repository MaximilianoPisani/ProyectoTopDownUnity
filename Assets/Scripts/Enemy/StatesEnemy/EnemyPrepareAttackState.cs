using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrepareAttackState : MonoBehaviour
{
    private EnemyController _enemy;
    private float _prepareTimer;
    private float _prepareDuration = 0.5f; 
    private IAttackStrategy _attackStrategy;

    public void EnterState(EnemyController enemy)
    {
        _enemy = enemy;
        _prepareTimer = 0f;
        _attackStrategy = _enemy.GetAttackStrategy();

        _enemy.agent.isStopped = true;
        _enemy.agent.velocity = Vector3.zero;
        _enemy.agent.ResetPath();

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
        float attackRange = _attackStrategy.GetAttackRange();

        if (distance > attackRange)
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