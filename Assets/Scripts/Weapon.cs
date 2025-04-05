using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string WeaponName;
    public AttackSystem AttackType;
    public Transform FirePoint;

    private void Awake()
    {
        
        if (AttackType == null)
        {
            AttackType = GetComponent<AttackSystem>();
        }
    }
}
