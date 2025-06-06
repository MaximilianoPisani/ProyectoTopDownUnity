using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackStrategy // #TEST
{
    float GetAttackRange();
    void ExecuteAttack(EnemyController enemy);
}