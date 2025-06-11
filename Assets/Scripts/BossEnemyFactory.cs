using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyFactory : IEnemyTypeFactory
{
    public IEnemyState CreateInitialState(EnemyController enemy)
    {
   
        enemy.SetAttackStrategy(new BossAttackStrategy(enemy));
        return new EnemyPatrolState(); 
    }
}
