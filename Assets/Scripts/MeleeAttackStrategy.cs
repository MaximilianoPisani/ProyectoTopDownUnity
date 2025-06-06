using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy // #TEST
{
    public float GetAttackRange()
    {
        return 1.5f;
    }

    public void ExecuteAttack(EnemyController enemy)
    {
        if (enemy.currentTarget == null) return;

        var melee = enemy.GetComponent<EnemyMeleeAttack>();
        melee?.UpdateAttack(); 
    }
}