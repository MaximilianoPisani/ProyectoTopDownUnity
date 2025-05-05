using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject CurrentWeapon;
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
        var weaponScript = CurrentWeapon.GetComponent<Weapon>();
        weaponScript.IsEquipped = true;
        CurrentWeapon.SetActive(true);

        _animator.SetBool("HasGun", true);

        var attackMelee = GetComponent<AttackMelee>();
        if (attackMelee != null)
        {
            attackMelee.canAttack = true;
        }
    }

    public void DropWeapon()
    {
        if (CurrentWeapon != null)
        {
            var weaponScript = CurrentWeapon.GetComponent<Weapon>();
            weaponScript.IsEquipped = false;
            CurrentWeapon.SetActive(false);

            _animator.SetBool("HasGun", false);

            var attackMelee = GetComponent<AttackMelee>();
            if (attackMelee != null)
            {
                attackMelee.canAttack = false;
            }

            PoolManager.Instance.ReturnToPool(CurrentWeapon);
            CurrentWeapon = null;
        }
    }
}