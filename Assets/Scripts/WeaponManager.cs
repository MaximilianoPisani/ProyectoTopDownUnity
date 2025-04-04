using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject meleeWeapon;
    public GameObject rangedWeapon;
    private GameObject _equippedWeapon;
    private GameObject _weaponInRange;
    private bool _isMelee;

    void Start()
    {
        EquipWeapon(meleeWeapon);
    }

    void Update()
    {
        // Cambiar de arma cuando presionamos "E"
        if (Input.GetKeyDown(KeyCode.E) && _weaponInRange != null)
        {
            EquipWeapon(_weaponInRange);
        }

        // Atacar con el arma equipada
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Detectar si hay un arma cerca
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            _weaponInRange = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == _weaponInRange)
        {
            _weaponInRange = null;
        }
    }

    void EquipWeapon(GameObject newWeapon)
    {
        if (_equippedWeapon != null)
        {
            DropWeapon(_equippedWeapon);
        }

        _equippedWeapon = newWeapon;
        _equippedWeapon.SetActive(true);
        _equippedWeapon.transform.SetParent(transform);
        _equippedWeapon.transform.localPosition = Vector3.zero;

        _isMelee = _equippedWeapon.CompareTag("MeleeWeapon");
    }

    void DropWeapon(GameObject weaponToDrop)
    {
        weaponToDrop.transform.SetParent(null);
        weaponToDrop.SetActive(true);
    }

    void Attack()
    {
        if (_equippedWeapon == null) return;

        if (_isMelee)
        {
            
        }
        else
        {
            Shoot();
        }
    }

    void Shoot()
    {
       
    }
}
