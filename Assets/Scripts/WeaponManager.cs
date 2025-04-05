using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon CurrentWeapon;

    public void EquipWeapon(Weapon newWeapon)
    {
        if (CurrentWeapon != null)
        {
            DropWeapon();
        }

        CurrentWeapon = newWeapon;
        CurrentWeapon.gameObject.SetActive(true);
        CurrentWeapon.transform.SetParent(transform); 
        CurrentWeapon.transform.localPosition = Vector3.zero;

    }

    public void DropWeapon()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.transform.SetParent(null);
            CurrentWeapon.gameObject.SetActive(true);
            CurrentWeapon = null;
        }
    }

    public void Attack()
    {
        if (CurrentWeapon != null && CurrentWeapon.AttackType != null)
        {
            CurrentWeapon.AttackType.Attack();
        }
    }
}