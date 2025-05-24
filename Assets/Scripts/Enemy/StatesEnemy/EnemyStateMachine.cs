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
        if (_enemy == null)
        Debug.LogError("EnemyController reference is missing ");
        
    }

    public void ChangeState(IEnemyState newState) // Changes the current state to a new one.
    {
        if (_currentState != null)
        {
            _currentState.ExitState();
        }

        _currentState = newState;

        if (_currentState != null)
        {
            _currentState.EnterState(_enemy);
        }
    }

    public void UpdateState() // Called every frame to update the logic of the current state.
    {
        if (_currentState != null)
        {
            _currentState.UpdateState();
        }
    }

    public IEnemyState GetCurrentState() // Returns the current active state of the enemy.
    {
        return _currentState;
    }
}