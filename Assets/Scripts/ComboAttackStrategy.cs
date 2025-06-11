using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttackStrategy : IAttackStrategy
{
    private readonly IAttackStrategy meleeStrategy;
    private readonly IAttackStrategy rangedStrategy;

    public ComboAttackStrategy(EnemyController enemy)
    {
        meleeStrategy = new MeleeAttackStrategy(enemy);
        rangedStrategy = new RangedAttackStrategy(enemy);
    }

    public float GetAttackRange()
    {
        return Mathf.Max(meleeStrategy.GetAttackRange(), rangedStrategy.GetAttackRange());
    }

    public void ExecuteAttack(EnemyController enemy)
    {
        if (enemy.currentTarget == null) return;

        float distance = Vector2.Distance(enemy.transform.position, enemy.currentTarget.position);

        if (distance <= meleeStrategy.GetAttackRange())
        {
            meleeStrategy.ExecuteAttack(enemy);
        }
        else if (distance <= rangedStrategy.GetAttackRange())
        {
            rangedStrategy.ExecuteAttack(enemy);
        }
    }
}