using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyTypeData", menuName = "Enemy/Enemy Type")]
public class EnemyTypeData : ScriptableObject // #TEST
{
    public EnemyType type;

    public IEnemyTypeFactory CreateFactory()
    {
        switch (type)
        {
            case EnemyType.Melee:
                return new MeleeEnemyFactory();
            case EnemyType.Ranged:
                return new RangedEnemyFactory();
            case EnemyType.Boss:
                return new BossEnemyFactory();
            default:
                Debug.LogError("Unsupported EnemyType: " + type);
                return null;
        }
    }
}

public enum EnemyType
{
    Melee,
    Ranged,
    Boss
}