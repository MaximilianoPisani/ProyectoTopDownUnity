using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyFactory : IEnemyTypeFactory // #TEST
{
    public IEnemyState CreateInitialState(EnemyController enemy)
    {

        enemy.SetAttackStrategy(new MeleeAttackStrategy(enemy));
        return new EnemyPatrolState();
    }
}