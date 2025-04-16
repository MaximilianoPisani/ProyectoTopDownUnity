using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string WeaponName;
    public AttackSystem AttackType;
    public Transform FirePoint;
    public bool IsEquipped = false;

    public enum WeaponType { Melee, Ranged }
    public WeaponType Type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !IsEquipped)
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                weaponManager.EquipWeapon(gameObject);
            }
        }
    }
}