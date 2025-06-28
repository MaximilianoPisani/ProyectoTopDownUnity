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
        if (_enemy == null)
        {
            Debug.LogError("EnemyController is null in EnemyIdleState");
            return;
        }

 
        Vector2 lastVelocity = _enemy.agent.velocity;
        if (lastVelocity.sqrMagnitude > 0.01f)
        {
            lastVelocity.Normalize();
            _enemy.animHandler.SetFloat("LastX", lastVelocity.x);
            _enemy.animHandler.SetFloat("LastY", lastVelocity.y);
            _enemy.UpdateLookDirection(lastVelocity); 
        }


        _enemy.SetMoving(false);
        _enemy.agent.isStopped = true;
        _enemy.agent.velocity = Vector3.zero;

        _enemy.animHandler?.SetBool("Moving", false);

        if (_enemy.animHandler.HasParameter("isAttackingMelee"))
            _enemy.animHandler.SetBool("isAttackingMelee", false);

        if (_enemy.animHandler.HasParameter("isAttackingRange"))
            _enemy.animHandler.SetBool("isAttackingRange", false);
    }


    public void UpdateState() // Called every frame while in the idle state
    {
       
    }

    public void ExitState() // Called when exiting the idle state
    {
        
    }
}