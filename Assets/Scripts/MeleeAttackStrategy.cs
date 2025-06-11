using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy // #TEST
{
    private EnemyMeleeAttack _meleeAttack;

    public MeleeAttackStrategy(EnemyController enemy)
    {
        _meleeAttack = enemy.GetComponent<EnemyMeleeAttack>();
        if (_meleeAttack == null)
            Debug.LogError("EnemyMeleeAttack component missing on enemy!");
    }

    public float GetAttackRange() => 1.5f;

    public void ExecuteAttack(EnemyController enemy)
    {
        if (enemy.currentTarget == null) return;
        _meleeAttack?.UpdateAttack();
    }
}