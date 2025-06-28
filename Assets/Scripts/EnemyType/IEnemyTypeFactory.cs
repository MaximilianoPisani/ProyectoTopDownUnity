using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyTypeFactory // #TEST
{
    IEnemyState CreateInitialState(EnemyController enemy);
}