using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public bool isEquipped = false;

    [SerializeField] private string _animatorFlag; // "HasSword" o "HasShotgun"

    private bool _isPlayerInRange = false;
    private GameObject _playerInRange;

    public string AnimatorFlag => _animatorFlag;

    private void Update()
    {
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryEquipWeapon();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            _playerInRange = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            _playerInRange = null;
        }
    }

    private void TryEquipWeapon()
    {
        if (isEquipped) return;

        WeaponManager weaponManager = _playerInRange.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            if (weaponManager.currentWeapon != null)
            {
                Weapon current = weaponManager.currentWeapon.GetComponent<Weapon>();
                if (current != null && current.weaponName == weaponName)
                {
                    Debug.Log("You already have this weapon equipped ");
                    return;
                }
            }

            weaponManager.EquipWeapon(gameObject);
        }
        else
        {
            Debug.LogWarning("Player does not have WeaponManager ");
        }
    }
}