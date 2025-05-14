using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void EnterState(EnemyMovement enemy);
    void UpdateState();
    void ExitState();
}