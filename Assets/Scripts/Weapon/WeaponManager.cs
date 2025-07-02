using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject meleeWeapon;
    public GameObject rangedWeapon;

    private AnimationControllerHandler _animator;

    [SerializeField] private Transform _weaponDropPoint;

    private void Start()
    {
        _animator = GetComponent<AnimationControllerHandler>();
        if (_animator == null)
            Debug.LogError("Missing AnimationControllerHandler");

        if (_weaponDropPoint == null)
            _weaponDropPoint = this.transform;
    }

    public void EquipWeapon(GameObject newWeapon)
    {
        Weapon weaponScript = newWeapon.GetComponent<Weapon>();
        if (weaponScript == null) return;

        GameObject oldWeapon = null;

        if (weaponScript.weaponType == WeaponType.Melee)
        {
            oldWeapon = meleeWeapon;
            meleeWeapon = newWeapon;
        }
        else if (weaponScript.weaponType == WeaponType.Ranged)
        {
            oldWeapon = rangedWeapon;
            rangedWeapon = newWeapon;
        }

        if (oldWeapon != null)
        {
            DropWeapon(oldWeapon);
        }

        weaponScript.isEquipped = true;
        _animator.SetBool(weaponScript.AnimatorFlag, true);
        newWeapon.SetActive(false);
    }

    public void DropWeapon(GameObject weaponToDrop)
    {
        if (weaponToDrop == null) return;

        Weapon weaponScript = weaponToDrop.GetComponent<Weapon>();
        if (weaponScript != null)
        {
            weaponScript.isEquipped = false;
            _animator.SetBool(weaponScript.AnimatorFlag, false);
        }

        weaponToDrop.transform.SetParent(null);
        weaponToDrop.transform.position = _weaponDropPoint.position;
        weaponToDrop.SetActive(true);

        Collider2D collider = weaponToDrop.GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = true;

        if (weaponScript.weaponType == WeaponType.Melee)
            meleeWeapon = null;
        else if (weaponScript.weaponType == WeaponType.Ranged)
            rangedWeapon = null;
    }
}