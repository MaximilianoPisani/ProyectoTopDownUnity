using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack_000_Data", menuName = "Data/Attack")]
public class AttackData : ScriptableObject
{
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damage;
    [SerializeField] private float _cooldown;

    public float AttackRange => _attackRange; // Read-only property for the attack range (used by enemies or players)
    public int Damage => _damage; // Read-only property for the amount of damage dealt (used by enemies or players)
    public float Cooldown => _cooldown; // Read-only property for the cooldown time between attacks (used by enemies or players)
}