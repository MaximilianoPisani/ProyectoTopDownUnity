using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack_000_Data", menuName = "Data/Attack")]
public class AttackData : ScriptableObject
{
    [SerializeField] private float _attackRange;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _stunDuration;

    public float AttackRange => _attackRange;
    public float Damage => _damage;
    public float Cooldown => _cooldown;
    public float StunDuration => _stunDuration;
}