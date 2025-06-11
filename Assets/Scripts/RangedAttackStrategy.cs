using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackStrategy : IAttackStrategy
{ //#TEST

    private EnemyRangedAttack _rangedAttack;

    public RangedAttackStrategy(EnemyController enemy)
    {
        _rangedAttack = enemy.GetComponent<EnemyRangedAttack>();
        if (_rangedAttack == null)
            Debug.LogError("AttackRanged component missing on enemy!");
    }

    public float GetAttackRange() => 5f;

    public void ExecuteAttack(EnemyController enemy)
    {
        if (enemy.currentTarget == null) return;
        _rangedAttack?.TryAttack();
    }
}