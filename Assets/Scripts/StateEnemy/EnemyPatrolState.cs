using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : IEnemyState
{
    private EnemyMovement _enemy;
    private float _waitTimer;
    private float _waitDuration = 1.5f;

    private int _currentPatrolIndex = 0;
    private Transform[] _patrolPoints;
    private NavMeshAgent _agent;

    public void EnterState(EnemyMovement enemy)
    {
        _enemy = enemy;
        _agent = _enemy.GetComponent<NavMeshAgent>();
        _patrolPoints = _enemy.GetPatrolPoints();

        if (_patrolPoints.Length == 0) return;

        _currentPatrolIndex = 0;
        GoToNextPoint();
    }

    public void UpdateState()
    {
        if (_patrolPoints.Length == 0) return;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _waitDuration)
            {
                _waitTimer = 0f;
                _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
                GoToNextPoint();
            }
        }
    }

    public void ExitState()
    {
        
    }

    private void GoToNextPoint()
    {
        if (_patrolPoints.Length == 0) return;

        Transform nextPoint = _patrolPoints[_currentPatrolIndex];
        _enemy.SetTarget(null); 
        _agent.SetDestination(nextPoint.position);
    }
}