using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : IEnemyState
{
    private EnemyController _enemy;
    private float _waitTimer;
    private float _waitDuration;
    private int _currentPatrolIndex;
    private Transform[] _patrolPoints;
    private NavMeshAgent _agent;

    public void EnterState(EnemyController enemy) // Called when the enemy enters the patrol state
    {

        _enemy = enemy;
        _agent = _enemy.agent;
        _patrolPoints = _enemy.GetPatrolPoints();
        _waitDuration = _enemy.GetPatrolWaitDuration();
        _enemy.animHandler.SetBool("Moving", true);
        _enemy.animHandler.SetBool("isAttacking", false);

        if (_patrolPoints.Length == 0) return;

        _currentPatrolIndex = 0;
        GoToNextPoint();
    }

    public void UpdateState() // Called every frame while the enemy is in the patrol state
    {
        if (_patrolPoints.Length == 0) return;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _enemy.animHandler.SetBool("Moving", false);
            _waitTimer += UnityEngine.Time.deltaTime;

            if (_waitTimer >= _waitDuration)
            {
                _waitTimer = 0f;
                _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
                GoToNextPoint();
                _enemy.animHandler.SetBool("Moving", true);
            }
        }

        UpdateDirectionAnimation();
    }

    public void ExitState() // Called when exiting the patrol state
    {
        _enemy.animHandler.SetBool("Moving", false);
    }

    private void GoToNextPoint() // Moves the enemy to the next patrol point
    {
        Transform nextPoint = _patrolPoints[_currentPatrolIndex];
        _enemy.SetTarget(null);
        _enemy.MoveTo(nextPoint.position);
    }

    private void UpdateDirectionAnimation() // Updates the animation direction based on the current movement velocity
    {
        Vector2 vel = _agent.velocity;
        if (vel.sqrMagnitude > 0.01f)
        {
            vel.Normalize();
            _enemy.animHandler.SetFloat("X", vel.x);
            _enemy.animHandler.SetFloat("Y", vel.y);
            _enemy.animHandler.SetFloat("LastX", vel.x);
            _enemy.animHandler.SetFloat("LastY", vel.y);
        }
    }
}