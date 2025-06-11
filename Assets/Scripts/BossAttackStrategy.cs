using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackStrategy : IAttackStrategy
{
    private readonly EnemyMeleeAttack meleeAttack;
    private readonly EnemyRangedAttack rangedAttack;
    private readonly Transform bossTransform;

    public BossAttackStrategy(EnemyController enemy)
    {
        meleeAttack = enemy.GetComponent<EnemyMeleeAttack>();
        rangedAttack = enemy.GetComponent<EnemyRangedAttack>();
        bossTransform = enemy.transform;
    }

    public float GetAttackRange()
    {
     
        return Mathf.Max(1.5f, 5f); 
    }

    public void ExecuteAttack(EnemyController enemy)
    {
        if (enemy.currentTarget == null) return;

        float distance = Vector3.Distance(bossTransform.position, enemy.currentTarget.position);

        if (distance <= 1.5f)
        {
            meleeAttack?.UpdateAttack();
        }
        else if (distance <= 5f)
        {
            rangedAttack?.SetTarget(enemy.currentTarget);
            rangedAttack?.TryAttack();
        }
    }
}