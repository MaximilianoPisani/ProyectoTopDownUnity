using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject meleeWeapon;
    public GameObject rangedWeapon;
    private GameObject _equippedWeapon;
    private bool _isMelee;

    public GameObject bulletPrefab;
    public Transform firePoint; // Punto donde se disparan los proyectiles

    private GameObject _weaponInRange; 

    void Start()
    {
        EquipWeapon(meleeWeapon); 
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && _weaponInRange != null)
        {
            EquipWeapon(_weaponInRange);
        }

        
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Detecta si el jugador está cerca de un arma
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            _weaponInRange = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Si el jugador se aleja del arma, la referencia desaparece
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

        // Busca si el arma tiene su propio FirePoint y lo usa
        Transform weaponFirePoint = _equippedWeapon.transform.Find("FirePoint");
        if (weaponFirePoint != null)
        {
            firePoint = weaponFirePoint;
        }
    }
    void DropWeapon(GameObject weaponToDrop)
    {
        weaponToDrop.transform.SetParent(null);
        weaponToDrop.SetActive(true);
        Debug.Log("Weapon dropped: " + weaponToDrop.name);
    }

    void Attack()
    {
        if (_equippedWeapon == null) return;

        if (_isMelee)
        {
            Debug.Log("Atacando con arma cuerpo a cuerpo: " + _equippedWeapon.name);
        }
        else
        {
            Debug.Log("Disparando con: " + _equippedWeapon.name);
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = firePoint.right * 10f; // Velocidad del disparo
    }
}