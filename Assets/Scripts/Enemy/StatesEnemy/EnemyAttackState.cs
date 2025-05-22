using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAttackState : IEnemyState
{
    private EnemyController _enemy;
    private EnemyMeleeAttack _meleeAttack;

    public void EnterState(EnemyController enemy) // Called when the enemy enters the attack state
    {
        _enemy = enemy;
        if (_enemy == null)
        {
            Debug.LogError("EnemyController is null in EnemyAttackState");
            return;
        }
        _meleeAttack = _enemy.GetComponent<EnemyMeleeAttack>();

        _enemy.agent.isStopped = true;
        _enemy.agent.velocity = Vector3.zero;
        _enemy.agent.ResetPath();
    }

    public void UpdateState() // Called every frame while in the attack state
    {
        if (_enemy.currentTarget == null)
        {
            _enemy.ChangeState(new EnemyPatrolState());
            return;
        }

        float distance = Vector2.Distance(_enemy.transform.position, _enemy.currentTarget.position);

        if (distance > _meleeAttack.AttackRange)
        {
            _enemy.ChangeState(new EnemyChaseState());
            return;
        }

       
        _meleeAttack.UpdateAttack(); 
    }

    public void ExitState() // Called when exiting the attack state
    {
        _enemy.agent.isStopped = false;
        _enemy.animHandler.SetBool("isAttacking", false);
    }
}