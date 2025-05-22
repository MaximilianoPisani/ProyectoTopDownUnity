using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public bool isEquipped = false;

    private void OnTriggerEnter2D(Collider2D other) // Called when another Collider2D enters this object's trigger area.
    {
        if (other.CompareTag("Player") && !isEquipped) // Checks if the player touches the weapon and if the weapon is not already equipped.
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                weaponManager.EquipWeapon(gameObject); // If so, equips the weapon to the player.
                PoolManager.Instance.ReturnToPool(gameObject); // Returns it to the object pool.
            }
            else
            {
                Debug.LogWarning("Player entered trigger but has no WeaponManager ");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // Called when another Collider2D exits this object's trigger area.
    {
        if (other.CompareTag("Player")) return;
    }
}