using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackStrategy : IAttackStrategy
{ //#TEST
    public float GetAttackRange()
    {
        return 5f; 
    }

    public void ExecuteAttack(EnemyController enemy)
    {
        var ranged = enemy.GetComponent<AttackRanged>();
        ranged?.TryAttack(); 
    }
}
