using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyIdleState : IEnemyState
{
    private EnemyController _enemy;

    public void EnterState(EnemyController enemy) // Called when entering the idle state
    {
        _enemy = enemy;
        _enemy.SetMoving(false);

        _enemy.animHandler.SetBool("Moving", false);
        _enemy.animHandler.SetBool("isAttacking", false);

       
        Vector2 lastVelocity = _enemy.agent.velocity;

        if (lastVelocity.sqrMagnitude > 0.01f)
        {
            lastVelocity.Normalize();
            _enemy.animHandler.SetFloat("LastX", lastVelocity.x);
            _enemy.animHandler.SetFloat("LastY", lastVelocity.y);
        }
    }

    public void UpdateState() // Called every frame while in the idle state
    {
       
    }

    public void ExitState() // Called when exiting the idle state
    {
        
    }
}