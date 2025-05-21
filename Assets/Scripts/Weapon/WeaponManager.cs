using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject currentWeapon;
    private AnimationControllerHandler _animator;

    private void Start()
    {
        _animator = GetComponent<AnimationControllerHandler>();
    }

    public void EquipWeapon(GameObject newWeapon) // Equip a new weapon.
    {
        if (currentWeapon != null)
        {
            DropWeapon();
        }

        currentWeapon = newWeapon;
        var weaponScript = currentWeapon.GetComponent<Weapon>();
        weaponScript.isEquipped = true;
        currentWeapon.SetActive(true);

        _animator.SetBool("HasGun", true);

        var attackMelee = GetComponent<AttackMelee>();
        if (attackMelee != null)
        {
            attackMelee.canAttack = true;
        }
    }

    public void DropWeapon() // Drop the currently equipped weapon.
    {
        if (currentWeapon != null)
        {
            var weaponScript = currentWeapon.GetComponent<Weapon>();
            weaponScript.isEquipped = false;
            currentWeapon.SetActive(false);

            _animator.SetBool("HasGun", false);

            var attackMelee = GetComponent<AttackMelee>();
            if (attackMelee != null)
            {
                attackMelee.canAttack = false;
            }

            PoolManager.Instance.ReturnToPool(currentWeapon);
            currentWeapon = null;
        }
    }
}