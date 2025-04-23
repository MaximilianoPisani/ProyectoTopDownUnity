using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string WeaponName;
    public AttackSystem AttackType;
    public bool IsEquipped = false;
    private bool isInRange = false; 

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !IsEquipped)
        {
            isInRange = true;
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                
                weaponManager.EquipWeapon(gameObject);
               
                PoolManager.Instance.ReturnToPool(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}