using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private IEnemyState _currentState;
    private EnemyController _enemy;

    private void Awake()
    {
        _enemy = GetComponent<EnemyController>();
    }

    public void SetInitialState(IEnemyState state)
    {
        if (_enemy == null)
            _enemy = GetComponent<EnemyController>();

        _currentState = state;
        _currentState.EnterState(_enemy);
    }

    public void ChangeState(IEnemyState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState?.EnterState(_enemy);
    }

    public void UpdateState()
    {
        _currentState?.UpdateState();
    }

    public IEnemyState GetCurrentState()
    {
        return _currentState;
    }
}