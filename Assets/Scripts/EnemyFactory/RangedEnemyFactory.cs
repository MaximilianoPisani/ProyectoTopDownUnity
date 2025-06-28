using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyFactory : IEnemyTypeFactory // #TEST
{
    public IEnemyState CreateInitialState(EnemyController enemy)
    {
        enemy.SetAttackStrategy(new RangedAttackStrategy(enemy));
        return new EnemyPatrolState();
    }
}