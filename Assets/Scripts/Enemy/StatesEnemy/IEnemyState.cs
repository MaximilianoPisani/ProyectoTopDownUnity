using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void EnterState(EnemyController enemy);
    void UpdateState();
    void ExitState();
}