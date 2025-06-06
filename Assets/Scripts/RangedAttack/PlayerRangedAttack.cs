using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : AttackRanged //#TEST
{
    private WeaponManager _weaponManager;
    [SerializeField] private RangedAttackData _attackData;
    protected override void Awake()
    {
        base.Awake(); 
        _weaponManager = GetComponent<WeaponManager>();
        if (_weaponManager == null)
        {
            Debug.LogWarning("WeaponManager not found on player.");
        }

        rangedAttackData = _attackData;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0) && IsCurrentWeaponRanged())
        {
            TryAttack();
        }
    }

    private bool IsCurrentWeaponRanged()
    {
        if (_weaponManager == null || _weaponManager.currentWeapon == null) return false;

       
        Weapon currentWeaponScript = _weaponManager.currentWeapon.GetComponent<Weapon>();
        if (currentWeaponScript == null) return false;

       
        return currentWeaponScript.weaponName == "Shotgun";
    }
}