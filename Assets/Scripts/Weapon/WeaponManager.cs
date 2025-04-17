using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject CurrentWeapon;
    public Transform WeaponHolder; 
    private AnimationControllerHandler _animator;

    private void Start()
    {
        _animator = GetComponent<AnimationControllerHandler>();
    }

    public void EquipWeapon(GameObject newWeapon)
    {
        if (CurrentWeapon != null)
        {
            DropWeapon();
        }

        CurrentWeapon = newWeapon;
        CurrentWeapon.transform.SetParent(WeaponHolder);
        CurrentWeapon.transform.localPosition = Vector3.zero;
        CurrentWeapon.transform.localRotation = Quaternion.identity;

        var weaponScript = CurrentWeapon.GetComponent<Weapon>();
        weaponScript.IsEquipped = true;
        CurrentWeapon.SetActive(true);

        if (weaponScript.Type == Weapon.WeaponType.Melee)
            _animator.TriggerAnimation("EquipSword");
        else if (weaponScript.Type == Weapon.WeaponType.Ranged)
            _animator.TriggerAnimation("EquipGun");
    }

    public void DropWeapon()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.GetComponent<Weapon>().IsEquipped = false;
            CurrentWeapon.transform.SetParent(null);
            CurrentWeapon.SetActive(false);

            PoolManager.Instance.ReturnToPool(CurrentWeapon); 
            CurrentWeapon = null;
        }
    }

    public void Attack()
    {
        if (CurrentWeapon == null) return;

        var weaponScript = CurrentWeapon.GetComponent<Weapon>();
        if (weaponScript != null && weaponScript.AttackType != null)
        {
            weaponScript.AttackType.Attack();

            if (weaponScript.Type == Weapon.WeaponType.Melee)
                _animator.TriggerAnimation("AttackSword");
            else if (weaponScript.Type == Weapon.WeaponType.Ranged)
                _animator.TriggerAnimation("ShootGun");
        }
    }
}