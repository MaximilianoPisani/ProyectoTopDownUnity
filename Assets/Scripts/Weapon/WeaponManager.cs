using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject currentWeapon;
    private AnimationControllerHandler _animator;

    [SerializeField] private Transform _weaponDropPoint;

    private void Start()
    {
        _animator = GetComponent<AnimationControllerHandler>();
        if (_animator == null)
        {
            Debug.LogError("Missing AnimationControllerHandler ");
        }

        if (_weaponDropPoint == null)
        {
            _weaponDropPoint = this.transform;
        }
    }

    public void EquipWeapon(GameObject newWeapon)
    {
        if (currentWeapon != null)
        {
            DropWeapon();
        }

        currentWeapon = newWeapon;
        Weapon weaponScript = currentWeapon.GetComponent<Weapon>();

        if (weaponScript != null)
        {
            weaponScript.isEquipped = true;
            _animator.SetBool(weaponScript.AnimatorFlag, true);
        }

        
        currentWeapon.SetActive(false);
    }

    public void DropWeapon()
    {
        if (currentWeapon != null)
        {
            Weapon weaponScript = currentWeapon.GetComponent<Weapon>();
            if (weaponScript != null)
            {
                weaponScript.isEquipped = false;
                _animator.SetBool(weaponScript.AnimatorFlag, false);
            }

       
            currentWeapon.transform.SetParent(null);
            currentWeapon.transform.position = _weaponDropPoint.position;

            currentWeapon.SetActive(true); 

            Collider2D collider = currentWeapon.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            currentWeapon = null;
        }
    }
}