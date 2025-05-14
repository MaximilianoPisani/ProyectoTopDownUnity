using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack_000_Data", menuName = "Data/Attack")]
public class AttackData : ScriptableObject
{
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damage;
    [SerializeField] private float _cooldown;

    public float AttackRange => _attackRange;
    public int Damage => _damage;
    public float Cooldown => _cooldown;
}